using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace iCareGames.Common.Core.AudioSystem
{
    public sealed class AudioPool {
        private readonly Transform _parent;
        private readonly AudioMixerGroup _mixer;
        private readonly ObjectPool<AudioSource> _pool;
        
        public AudioPool(Transform parent, AudioMixerGroup mixer, int defaultValue, int maxValue) {
            _parent = parent;
            _mixer = mixer;
            _pool = new ObjectPool<AudioSource>(OnCreate, OnGet, OnRelease, OnSourceDestroy, true,defaultValue, maxValue);
        }

        private AudioSource OnCreate() {
            var source = new GameObject("PooledAudioSource").AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _mixer;
            source.transform.SetParent(_parent);
            return source;
        }
        private static void OnGet(AudioSource obj) {
            obj.volume = 0;
            obj.gameObject.SetActive(true);
        }
        private static void OnRelease(AudioSource obj) {
            obj.gameObject.SetActive(false);
            obj.volume = 0;
            obj.Stop();
        }
        private static void OnSourceDestroy(AudioSource obj) {
            Object.Destroy(obj.gameObject);
        }

        public AudioSource Get() => _pool.Get();
        public void Release(AudioSource source) => _pool.Release(source);
    }
}
