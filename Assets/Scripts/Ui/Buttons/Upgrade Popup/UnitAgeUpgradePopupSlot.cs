using System;
using BackEnd.Enums;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UnitAgeUpgradePopupSlot : MonoBehaviour
    {
        public delegate void AgeUpgradeDelegate(UnitType unitType, Sprite sprite);
        public static event AgeUpgradeDelegate OnUnitAgeUpgrade;
        
        [Tooltip("Which unit should be upgraded")]
        [SerializeField] private UnitType _unitType;

        [Tooltip("Age stage to be upgraded to")]
        [SerializeField] private AgeStageType _ageStage;
        
        [Tooltip("Prefab that get instantiated after the upgrade")]
        [SerializeField] private GameObject _upgradedUnitPrefab;
        
        [Tooltip("Sprite that get changed in the deploy button after the upgrade")]
        [SerializeField] private Sprite _upgradedButtonSprite;
        
        [FormerlySerializedAs("_range")]
        [Header("Stat to upgrade")]
        [Tooltip("How far the unit detects opposite unit")]
        public int Range;
        [Tooltip("The speed Of the Unit")]
        public float Speed = 1;
        [Tooltip("The Health Of the Unit")]
        public int Health = 1;
        
        [Tooltip("Strength to add to MinStrength.")]
        [Min(0)]
        public int MinStrength = 1;
        
        [Tooltip("Strength to add to MaxStrength.")]
        [Min(0)]
        public int MaxStrength = 1;
        
        [Tooltip("Percentage reduction applied to initial attack delay after upgrade.\n" +
                 "For example, a value of 20 reduces the delay by 20%.")]
        public float AttackDelayReductionPercent = 1;

        private UnitData _unitData;
        
        public UnitType Type => _unitType;
        public AgeStageType AgeStage => _ageStage;
        private void Awake()
        {
            _unitData = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
        }


        //TODO: update deploy button sprite & test with differnt prefabs
        public void UpgradeUnitAge()
        {
            
            //---BaseParams---
            _unitData.Range += Range;
            _unitData.Speed += Speed;
            _unitData.Health += Health;
            _unitData.MinStrength += MinStrength;
            _unitData.MaxStrength += MaxStrength;
            _unitData.InitialAttackDelay *= 1f - (AttackDelayReductionPercent / 100f);
            _unitData.InitialAttackDelay = Mathf.Max(0.1f, _unitData.InitialAttackDelay);

            //---Prefab---
            _unitData.Prefab = _upgradedUnitPrefab;

            //---AgeStage---
            _unitData.AgeStage = _ageStage;
            
            FinalizeUpgrade();
        }

        private void FinalizeUpgrade()
        {
            UpgradeDataStorage.Instance.RegisterAgeUpgrade(_unitType, _ageStage);
            UpgradePopup.Instance.BlockRaycasts(false);

            UIEffects.ShrinkAndDestroy(transform, 1.2f, 0, () =>
            {
                UpgradePopup.Instance.HidePopup();
                OnUnitAgeUpgrade?.Invoke(_unitType, _upgradedButtonSprite);
            });
        }
    }
}