using UnityEngine;

namespace iCareGames.Common.Core.AudioSystem
{
    public class AudioPlayer : MonoBehaviour {

        public void PlaySfx3D(AudioSO audiodata) {
            AudioManager.PlaySfx(audiodata, transform.position, false);
        }
        public void PlaySfx2D(AudioSO audiodata) {
            AudioManager.PlaySfx(audiodata, transform.position, true);
        }

        public void PlayLoop3D(AudioSO audioData) {
            AudioManager.PlayLoop(audioData, transform.position, false);
        }
        public void PlayLoop2D(AudioSO audioData) {
            AudioManager.PlayLoop(audioData, transform.position, true);
        }

        public void StopSfx(AudioSO audioData) {
            AudioManager.StopSfx(audioData);
        }

        public void StopLoop(AudioSO audioData) {
            AudioManager.StopLoop(audioData);
        }
        
    }
}
