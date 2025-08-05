using BackEnd.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace BackEnd.Utilities
{
    [System.Serializable]
    public class SfxEntry
    {
        [FormerlySerializedAs("type")] [SerializeField] private SfxType _type;
        public SfxType Type => _type;

        [FormerlySerializedAs("audioClip")] [SerializeField] private AudioClip _audioClip;
        public AudioClip AudioClip => _audioClip;

        [FormerlySerializedAs("volume")] [SerializeField, Range(0f, 1f)] private float _volume = 1f;
        public float Volume => _volume;
    }
}