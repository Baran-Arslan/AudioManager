using UnityEngine;

namespace _Common.iCare_AudioManager {
    [CreateAssetMenu(fileName = "AudioSO", menuName = "iCareGames/Audio", order = 0)]
    public class AudioSO : ScriptableObject {
        public AudioClip[] Clips;
        [Range(0f, 1f)]
        public float Volume = 1f;
        [Range(-3f, 3f)]
        public float Pitch = 1f;
        [Range(0f, 2.5f)]
        public float PitchRandomness;
        [Range(0, 0.8f)]
        public float VolumeRandomness;
    }
}