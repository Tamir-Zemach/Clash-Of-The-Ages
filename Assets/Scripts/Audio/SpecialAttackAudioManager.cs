using System;
using BackEnd.Enums;
using Assets.Scripts.Data;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using Special_Attacks;
using turrets;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SpecialAttackAudioManager : MonoBehaviour
    {
        private SpecialAttackBaseBehavior _specialAttackBaseBehavior;
        private AudioSource _audioSource;
        private SpecialAttackData _specialAttackData;



        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _specialAttackBaseBehavior = GetComponent<SpecialAttackBaseBehavior>();
            _audioSource.playOnAwake = false;
        }
        
        private void Start()
        {
            _specialAttackData = _specialAttackBaseBehavior.SpecialAttack;
            SubscribeRelevantAttack(_specialAttackData.Type);
        }

        private void SubscribeRelevantAttack(SpecialAttackType  type)
        {
            switch (type)
            {
                case SpecialAttackType.DestroyPath:
                    var meteorRain = GetComponent<MeteorRain>();
                    meteorRain.OnMeteorSpawned += () => AudioManager.PlayAudio(_audioSource, _specialAttackData, SfxType.Attacking);
                    break;
                case SpecialAttackType.Invisibility:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        
    }
}