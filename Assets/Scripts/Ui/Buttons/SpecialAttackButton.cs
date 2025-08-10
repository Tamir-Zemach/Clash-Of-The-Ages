using System.Collections.Generic;
using BackEnd.Enums;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using DG.Tweening;
using Managers;
using Special_Attacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui.Buttons
{
    public class SpecialAttackButton : ButtonWithCost, IImageSwitchable<SpecialAttackType>
    {
        
        public UnityEvent OnTimerStarted;
        public UnityEvent OnTimerStoped;
        [field: SerializeField] public SpecialAttackType Type {  get; private set; }

        [SerializeField] private float _specialAttackTimer;
        
        private Slider _cooldownSlider;

        private SpecialAttackSpawnPos _specialAttackSpawnPos;

        private SpecialAttackData _specialAttack;

        private Image _image;
        
        private Tween _cooldownTween;
        
        
        
        private void Start()
        {
            GetData();
        }
        

        private void GetData()
        {
            _specialAttack = GameDataRepository.Instance.FriendlySpecialAttacks.GetData(Type);
            _cooldownSlider = GetComponentInChildren<Slider>();
            _image = GetComponent<Image>();
            LevelLoader.Instance.OnSceneChanged += GetSpawnPos;
            UiAgeUpgrade.Instance.OnUiRefreshSpecialAttack += UpdateSprite;
            GetSpawnPos();
            ResetTimer();
        }

        private void GetSpawnPos()
        {
            _specialAttackSpawnPos = FindAnyObjectByType<SpecialAttackSpawnPos>(); 
        }

        
        private void UpdateSprite(List<SpriteEntries.SpriteEntry<SpecialAttackType>> spriteMap)
        {
            foreach (var s in spriteMap)
            {
                if (s.GetKey() == Type)
                {
                    var newSprite = s.GetSprite();
                    if (_image != null && newSprite != null)
                    {
                        _image.sprite = newSprite;
                    }
                    break; 
                }
            }
        }

        public void PerformSpecialAttack()
        {
            if (!CanPerformAttack()) return;
            PlayerCurrency.Instance.SubtractMoney(Cost);
            _specialAttackSpawnPos.IsSpecialAttackAccruing = true;
            SpawnSpecialAttack();
            ResetTimer();
        }

        private void SpawnSpecialAttack()
        {
            var specialAttack = Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.transform.position, _specialAttackSpawnPos.transform.rotation);
            var behaviour = specialAttack.GetComponent<SpecialAttackBaseBehavior>();

            if (behaviour != null)
            {
                behaviour.Initialize(_specialAttack, _specialAttackSpawnPos);
            }
            else
            {
                Debug.LogWarning("SpecialAttackBaseBehaviour not found on spawned enemy prefab.");
            }

        }

        private bool CanPerformAttack()
        {
            return PlayerCurrency.Instance.HasEnoughMoney(Cost)
                   && !_specialAttackSpawnPos.IsSpecialAttackAccruing
                   && _cooldownSlider.value >= 1f;
        }
        
        
        private void ResetTimer()
        {
            _cooldownSlider.value = 0f;
            OnTimerStarted?.Invoke();

            _cooldownTween = UIEffects.AnimateSliderFill(_cooldownSlider, 1f, _specialAttackTimer, () =>
            {
                UIEffects.ApplyGraphicFeedback(
                    graphic: _image,
                    shakeGraphic: true, shakeDuration: 0.3f,
                    changeScale: true, scaleMultiplier: 1.2f, scaleChangeDuration: 0.2f,
                    onComplete: () => { OnTimerStoped?.Invoke(); }
                );
            });
        }

        #region GameLifecycle
            //TODO: make a helper/use the base class "InGameObject
            private void OnEnable()
            {
                GameStates.Instance.OnGamePaused += PauseCooldown;
                GameStates.Instance.OnGameResumed += ResumeCooldown;
                GameStates.Instance.OnGameEnded += CancelCooldown;
                GameStates.Instance.OnGameReset += CancelCooldown;
            }

            private void OnDisable()
            {
                GameStates.Instance.OnGamePaused -= PauseCooldown;
                GameStates.Instance.OnGameResumed -= ResumeCooldown;
                GameStates.Instance.OnGameEnded -= CancelCooldown;
                GameStates.Instance.OnGameReset -= CancelCooldown;
            }

            private void PauseCooldown()
            {
                _cooldownTween?.Pause();
            }

            private void ResumeCooldown()
            {
                _cooldownTween?.Play();
            }

            private void CancelCooldown()
            {
                _cooldownTween?.Kill();
                _cooldownTween = null;
                _cooldownSlider.value = 0f;
            }

        #endregion
    
    }
}
