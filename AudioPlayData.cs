using UnityEngine;

namespace _Common.iCare_AudioManager {
    public struct AudioPlayData {
        public AudioSO AudioSO;
        public Vector3 PlayPosition;
        public Transform Parent;
        public bool Loop;
        public bool Is2D;
        public AudioType AudioType;
    }
}