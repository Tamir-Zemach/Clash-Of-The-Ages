using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
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
            if (!CanPreformAttack()) return;
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

        private bool CanPreformAttack()
        {
            return PlayerCurrency.Instance.HasEnoughMoney(Cost)
                   && !_specialAttackSpawnPos.IsSpecialAttackAccruing
                   && _cooldownSlider.value >= 1f;
        }
        
        
        private void ResetTimer()
        {
            _cooldownSlider.value = 0f;
            OnTimerStarted?.Invoke();

            UIEffects.AnimateSliderFill(_cooldownSlider, 1f, _specialAttackTimer, () =>
            {
                UIEffects.ApplyGraphicFeedback(
                    graphic: _image,
                    shakeGraphic: true, shakeDuration: 0.3f, 
                    changeScale: true, scaleMultiplier: 1.2f, scaleChangeDuration: 0.2f,
                    onComplete: () => {  OnTimerStoped?.Invoke(); }
                );
               
            });
        }
    
    
    }
}
