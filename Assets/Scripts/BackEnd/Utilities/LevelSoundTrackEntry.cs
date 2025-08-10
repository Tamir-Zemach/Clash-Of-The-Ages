using BackEnd.Enums;
using UnityEngine;

namespace BackEnd.Utilities
{
    [System.Serializable]
    public class LevelSoundTrackEntry
    {
        [field: SerializeField] public LevelType Level { get; set; }
        [field: SerializeField] public AudioClip Soundtrack { get; set; }
    }
}
