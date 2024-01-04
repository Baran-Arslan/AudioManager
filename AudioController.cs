using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace iCareGames.Common.Core.AudioSystem
{
    public sealed class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup sfxMixer;
        private AudioPool _sfxPool;
        [SerializeField] private AudioMixerGroup musicMixer;
        private AudioPool _musicPool;
        [SerializeField] private AudioMixerGroup loopMixer;
        private AudioPool _loopPool;

        private Dictionary<AudioSO, AudioSource> _playingSfxDictionary = new Dictionary<AudioSO, AudioSource>();
        private Dictionary<AudioSO, AudioSource> _playingMusicDictionary = new Dictionary<AudioSO, AudioSource>();
        private Dictionary<AudioSO, AudioSource> _playingLoopDictionary = new Dictionary<AudioSO, AudioSource>();

        private void Awake()
        {
            _sfxPool = new AudioPool(transform, sfxMixer, 20, 80);
            _musicPool = new AudioPool(transform, musicMixer, 3, 30);
            _loopPool = new AudioPool(transform, loopMixer, 5, 30);
        }

        public void PlaySfx(AudioSO audioData, Vector3 playPosition = default, bool is2D = true, float smoothVolumeTime = 0, bool canAddToDictionary = false)
        {
            AudioSource source = _sfxPool.Get();

            if (canAddToDictionary)
            {
                if (AddToDictionary(_playingSfxDictionary, source, audioData))
                    PlayWithSettings(source, audioData, playPosition, is2D, false, smoothVolumeTime);
                else
                {
                    _sfxPool.Release(source);
                }
                return;
            }
            PlayWithSettings(source, audioData, playPosition, is2D, false, smoothVolumeTime);
            StartCoroutine(RelaseIn(audioData.Clip.length, source, _sfxPool));
        }

        public void StopSfx(AudioSO audioData, float smoothVolumeTime = 0)
        {
            var source = RemoveFromDictionary(_playingSfxDictionary, audioData);
            if (source != null)
            {
                StopSound(source, _sfxPool, smoothVolumeTime);
            }
        }

        public void PlayLoop(AudioSO audioData, Vector3 playPosition = default, bool is2D = true, float smoothVolumeTime = 0)
        {
            AudioSource source = _loopPool.Get();

            if (AddToDictionary(_playingLoopDictionary, source, audioData))
                PlayWithSettings(source, audioData, playPosition, is2D, true, smoothVolumeTime);
            else
            {
                _loopPool.Release(source);
            }
        }

        public void StopLoop(AudioSO audioData, float smoothVolumeTime = 0)
        {
            var source = RemoveFromDictionary(_playingLoopDictionary, audioData);
            if (source != null)
            {
                StopSound(source, _loopPool, smoothVolumeTime);
            }
        }

        public void PlayMusic(AudioSO audioData, float smoothVolumeTime = 0)
        {
            AudioSource source = _musicPool.Get();

            if (AddToDictionary(_playingMusicDictionary, source, audioData))
                PlayWithSettings(source, audioData, transform.position, true, true, smoothVolumeTime);
            else
                _musicPool.Release(source);
        }

        public void StopMusic(AudioSO audioData, float smoothVolumeTime = 0)
        {
            var source = RemoveFromDictionary(_playingMusicDictionary, audioData);
            if (source != null)
            {
                StopSound(source, _musicPool, smoothVolumeTime);
            }
        }

        private bool AddToDictionary(Dictionary<AudioSO, AudioSource> dictionary, AudioSource source, AudioSO audioData)
        {
            if (audioData == null)
            {
                return false;
            }
            if (dictionary.ContainsKey(audioData))
            {
                return false;
            }
            dictionary.Add(audioData, source);
            return true;
        }

        private AudioSource RemoveFromDictionary(Dictionary<AudioSO, AudioSource> dictionary, AudioSO audioData)
        {
            if (audioData == null)
            {
                return null;
            }
            if (dictionary.ContainsKey(audioData))
            {
                AudioSource source = dictionary[audioData];
                dictionary.Remove(audioData);
                return source;
            }
            return null;
        }

        private void PlayWithSettings(AudioSource source, AudioSO audioData, Vector3 playPosition = default, bool is2D = true, bool isLoop = false, float smoothVolumeTime = 0)
        {
            if (audioData == null)
            {
                return;
            }
            ApplySettings(source, audioData, playPosition, is2D, true, smoothVolumeTime);
            source.Play();
        }

        private void ApplySettings(AudioSource source, AudioSO audioData, Vector3 playPosition = default, bool is2D = true, bool isLoop = false, float smoothVolumeTime = 0)
        {
            source.transform.position = playPosition;
            source.spatialBlend = is2D ? 0 : 1;
            source.clip = audioData.Clip;
            float targetVolume = audioData.DefaultVolume + Random.Range(-audioData.VolumeRandomness, audioData.VolumeRandomness);
            source.DOFade(targetVolume, smoothVolumeTime);
            source.pitch = audioData.DefaultPitch + Random.Range(-audioData.PitchRandomness, audioData.PitchRandomness);
            source.priority = audioData.Priority;
            source.loop = isLoop;
        }

        private IEnumerator RelaseIn(float stopTime, AudioSource source, AudioPool pool)
        {
            yield return new WaitForSecondsRealtime(stopTime);
            StopSound(source, pool);
        }

        private void StopSound(AudioSource source, AudioPool pool, float smoothVolumeTime = 0)
        {
            if (smoothVolumeTime > 0)
                source.DOFade(0, smoothVolumeTime).OnComplete(() =>
                {
                    source.Stop();
                    pool.Release(source);
                });
            else
            {
                source.Stop();
                pool.Release(source);
            }
        }
    }
}
