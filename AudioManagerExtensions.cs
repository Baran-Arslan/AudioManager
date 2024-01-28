using UnityEngine;

namespace _Common.iCare_AudioManager {
    public static class AudioManagerExtensions {
        
        public static void Play2D(this AudioSO audioSo, bool isLoop = false, AudioType audioType = AudioType.SFX) {
            var audioPlayData = new AudioPlayData {
                AudioSO = audioSo,
                Is2D = true,
                Loop = isLoop,
                AudioType = audioType
            };
        }
        public static void Play3D(this AudioSO audioSo, Vector3 playPosition, Transform parent = null, bool isLoop = false, AudioType audioType = AudioType.SFX) {
            var audioPlayData = new AudioPlayData {
                AudioSO = audioSo,
                PlayPosition = playPosition,
                Parent = parent,
                Is2D = false,
                Loop = isLoop,
                AudioType = audioType
            };
        }
        public static void Stop(this AudioSO audioSo) {
            AudioManager.Instance.Stop(audioSo);
        }
    }
}