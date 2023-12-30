using UnityEngine;

namespace iCareGames.Common.Core.AudioSystem {
    [CreateAssetMenu(fileName = "AudioSO", menuName = "iCareGames/AudioSystem/AudioSO")]
    public class AudioSO : ScriptableObject {
        [SerializeField] private AudioClip[] Clips;
        public AudioClip Clip => Clips[Random.Range(0, Clips.Length)];
        
        [Range(0, 1f)]
        public float DefaultVolume = 1;
        public float DefaultPitch { get; private set; } = 1f;
        
        
        [Range(0, 256)]
        public int Priority = 128;

        [Range(0, 0.5f)] 
        public float VolumeRandomness = 0;
        [Range(0, 3)]
        public float PitchRandomness = 0;
    }
}