using Animations;
using BackEnd.Enums;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using units.Behavior;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class UnitAudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitAnimationActions _unitAnimationActions;
        private UnitHealthManager _unitHealthManager;
        private UnitData _unitData;
        

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
            _unitHealthManager = GetComponent<UnitHealthManager>();
            _unitAnimationActions = GetComponentInChildren<UnitAnimationActions>();
            _audioSource.playOnAwake = false;

        }
        private void Start()
        {
            _unitData = _unitBaseBehaviour.Unit;

            if (_unitData == null)
            {
                Debug.LogError("UnitData is still null in Start(). Audio will not hook.");
                return;
            }

            if (_unitAnimationActions != null)
                _unitAnimationActions.OnAttack += () => AudioManager.PlayAudio(_audioSource, _unitData, SfxType.Attacking);

            if (_unitHealthManager == null) return;
            _unitHealthManager.OnHealthChanged += () => AudioManager.PlayAudio(_audioSource, _unitData, SfxType.GettingHit);
            _unitHealthManager.OnDying += () => AudioManager.PlayAudio(_audioSource, _unitData, SfxType.Dying);
        }

    }
}

//TODO: Dying clip is deleted when the object gets destroyed 
