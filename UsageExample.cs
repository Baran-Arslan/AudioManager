using UnityEngine;

namespace _Common.iCare_AudioManager {
    public class UsageExample : MonoBehaviour {
        [SerializeField] private AudioSO gameMusic;
        [SerializeField] private AudioSO takeHitSfx;
        [SerializeField] private AudioSO takeHitVoice;

        private void Awake() {
            gameMusic.Play2D(true, AudioType.Music);
            takeHitSfx.Play3D(transform.position,transform ,false, AudioType.SFX);
            takeHitVoice.Play2D(false, AudioType.Voice);
        }
    }
}
