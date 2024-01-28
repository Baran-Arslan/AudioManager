using UnityEngine;
using UnityEngine.Pool;

namespace _Common.iCare_AudioManager {
    public class AudioSourcePool {
        
        private readonly ObjectPool<AudioSource> _pool;
        private readonly Transform _parent;
        

        public AudioSourcePool(Transform parent, int preloadAmount = 5) {
            _pool = new ObjectPool<AudioSource>(OnCreate, OnGet, OnRelease, OnClear, true, 30);
            _parent = parent;
            Preload(preloadAmount);
        }
        
        public void Release(AudioSource source) {
            _pool.Release(source);
        }
        public AudioSource Get() {
            return _pool.Get();
        }
        

        private void Preload(int amount) {
            var sources = new AudioSource[amount];
            for (var i = 0; i < amount; i++) {
                sources[i] = _pool.Get();
            }
            for (var i = 0; i < amount; i++) {
                _pool.Release(sources[i]);
            }
        }
        private AudioSource OnCreate() {
            var source = new GameObject("PooledAudioSource").AddComponent<AudioSource>();
            source.transform.SetParent(_parent);
            return source;
        }
        private static void OnGet(AudioSource obj) {
            obj.gameObject.SetActive(true);
        }
        private void OnRelease(AudioSource obj) {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parent);
        }
        private static void OnClear(AudioSource obj) {
            Object.Destroy(obj);
        }
    }
}