using System;
using BackEnd.Enums;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Utilities;
using turrets;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class TurretAudioManager : MonoBehaviour
    {
        private TurretBaseBehavior _turretBaseBehavior;
        private TurretData _turretData;
        private AudioSource _audioSource;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _turretBaseBehavior = GetComponent<TurretBaseBehavior>();
            _audioSource.playOnAwake = false;
            
        }
        
        private void Start()
        {
            _turretData = _turretBaseBehavior.Turret;
            
            _turretBaseBehavior.OnAttack += () => AudioManager.PlayAudio(_audioSource, _turretData, SfxType.Attacking);

        }
        
    }
}
