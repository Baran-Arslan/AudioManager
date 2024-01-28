using System.Collections;
using System.Collections.Generic;
using _Common.iCare_Core;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Audio;
using Logger = _Common.iCare_Core.Logger;

namespace _Common.iCare_AudioManager {
    public sealed class AudioManager : UnitySingeleton<AudioManager> {
        [SerializeField] private int preLoadAmount = 30;

        [SerializeField] private AudioMixerGroup musicMixerGroup;
        [SerializeField] private AudioMixerGroup sfxMixerGroup;
        [SerializeField] private AudioMixerGroup voiceMixerGroup;

        private AudioSourcePool _pool;
        private Dictionary<AudioType, AudioMixerGroup> _mixerGroups;
        private readonly Dictionary<AudioSO, AudioSource> _playingSources = new Dictionary<AudioSO, AudioSource>();

        private void Awake() {
            _pool = new AudioSourcePool(transform, preLoadAmount);

            _mixerGroups = new Dictionary<AudioType, AudioMixerGroup> {
                { AudioType.Music, musicMixerGroup },
                { AudioType.SFX, sfxMixerGroup },
                { AudioType.Voice, voiceMixerGroup }
            };
        }
        
        public void Play(AudioPlayData audioPlayData) {
            if (audioPlayData.AudioSO.Clips.IsNullOrEmpty()) {
                Debug.LogWarning("There is no clip in this AudioSO" + audioPlayData.AudioSO.name, gameObject);
                return;
            }
            var source = _pool.Get();
            var randomClip = audioPlayData.AudioSO.Clips[Random.Range(0, audioPlayData.AudioSO.Clips.Length)];
            source.transform.position = audioPlayData.PlayPosition;
            source.transform.SetParent(audioPlayData.Parent);
            source.clip = randomClip;
            var volumeRandomness = Random.Range(-audioPlayData.AudioSO.VolumeRandomness, audioPlayData.AudioSO.VolumeRandomness);
            var pitchRandomness = Random.Range(-audioPlayData.AudioSO.PitchRandomness, audioPlayData.AudioSO.PitchRandomness);
            source.volume = audioPlayData.AudioSO.Volume + volumeRandomness;
            source.pitch = audioPlayData.AudioSO.Pitch + pitchRandomness;
            source.outputAudioMixerGroup = _mixerGroups[audioPlayData.AudioType];
            source.loop = audioPlayData.Loop;
            source.spatialBlend = audioPlayData.Is2D ? 0 : 1;
            source.Play();
            if (audioPlayData.Loop) {
                _playingSources.Add(audioPlayData.AudioSO, source);
            }
            else {
                StartCoroutine(ReleaseCoroutine(source, source.clip.length));
            }
        }

        [Button]
        public void Stop(AudioSO audioSO) {
            if (!_playingSources.ContainsKey(audioSO)) {
                Logger.LogWarning("There is no audio playing with this AudioSO", gameObject);
                return;
            }

            var source = _playingSources[audioSO];
            source.Stop();
            _pool.Release(source);
            _playingSources.Remove(audioSO);
        }

        private IEnumerator ReleaseCoroutine(AudioSource source, float delay) {
            yield return new WaitForSeconds(delay);
            _pool.Release(source);
        }
    }
}