using BackEnd.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts.BackEnd.Utilities
{
    [System.Serializable]
    public class LevelSoundTrackEntry
    {
        [field: SerializeField] public LevelType Level { get; set; }
        [field: SerializeField] public AudioClip Soundtrack { get; set; }
    }
}
