using UnityEngine;

namespace iCareGames.Common.Core.AudioSystem
{
    [RequireComponent(typeof(AudioController))]
    public class AudioManager : MonoBehaviour {
        private static AudioController _audioController;

        private void Awake() {
            _audioController = GetComponent<AudioController>();
        }

        public static void PlaySfx(AudioSO audioData, Vector3 playPosition = default, bool is2D = true, float smoothVolumeTime = 0, bool canAddToDictionary = false) {
            _audioController.PlaySfx(audioData, playPosition, is2D, smoothVolumeTime, canAddToDictionary);
        }
        public static void StopSfx(AudioSO audioData, float smoothVolumeTime = 0) {
            _audioController.StopSfx(audioData, smoothVolumeTime);
        }
        public static void PlayLoop(AudioSO audioData, Vector3 playPosition = default, bool is2D = true, float smoothVolumeTime = 0) {
            _audioController.PlayLoop(audioData, playPosition, is2D, smoothVolumeTime);
        }
        public static void StopLoop(AudioSO audioData, float smoothVolumeTime = 0) {
            _audioController.StopLoop(audioData, smoothVolumeTime);
        }
        public static void PlayMusic(AudioSO audioData) {
            _audioController.PlayMusic(audioData);
        }
        public static void StopMusic(AudioSO audioData, float smoothVolumeTime = 0) {
            _audioController.StopMusic(audioData, smoothVolumeTime);
        }
        
    }
}
