using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.UI;
using static BackEnd.Utilities.SpriteKeys; 


namespace Ui.Buttons.Upgrade_Buttons
{
    public class UnitUpgradeButton : ButtonWithCost, IImageSwitchable<UnitType>
    {
        [Tooltip("Which unit should be upgraded")]
        [SerializeField] private UnitType _unitType;

        [Tooltip("What Stat to Upgrade")]
        [SerializeField] private StatType _statType;

        [Header("Upgrade stats Settings")]
        [Tooltip("stat bonus added per upgrade")]
        [SerializeField] private int _statBonus;

        [Header("Upgrade stats Settings")]
        [Tooltip("Percentage to reduce attack delay per upgrade " +
                 "(i.e., increase attack speed)." +
                 " Accepts decimal values.")]
        [SerializeField] private float _attackDelayReductionPercent;
        

        [Tooltip("Incremental increase in stat upgrade cost after each upgrade")]
        [SerializeField] private int _statCostInc;

        private UnitData _unit;

        private Image _image;
        
        public UnitType Type => _unitType;

        private void Start()
        {
            GetData();
        }

        private void GetData()
        {
            _unit = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
            _image = GetComponent<Image>();
            UiAgeUpgrade.Instance.OnUiRefreshUpgradeUnits += UpdateSprite;
        }


        private void UpdateSprite(List<SpriteEntries.SpriteEntry<UnitUpgradeButtonKey>> spriteMap)
        {
            foreach (var s in spriteMap)
            {
                var key = s.GetKey();

                if (key.StatType == _statType && key.UnitType == _unitType)
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


        public void UpgradeStat()
        {
            if (!PlayerCurrency.Instance.HasEnoughMoney(Cost)) return;
            PlayerCurrency.Instance.SubtractMoney(Cost);
            ApplyUpgrade(_unit);
            Cost += _statCostInc;
            HoverCostDisplay.Instance.UpdateCost(Cost);
        }

        private void ApplyUpgrade(UnitData unit)
        {
            switch (_statType)
            {
                case StatType.Strength:
                    unit.MinStrength += _statBonus;
                    unit.MaxStrength += _statBonus;
                    break;

                case StatType.Health:
                    unit.Health += _statBonus;
                    break;

                case StatType.Range:
                    unit.Range += _statBonus;
                    break;
                case StatType.AttackSpeed:
                    unit.InitialAttackDelay *= 1f - (_attackDelayReductionPercent / 100f);
                    break;
                default:
                    Debug.LogWarning("Unknown stat type: " + _statType);
                    break;
            }
        }

#if UNITY_EDITOR
        public static class FieldNames
        {
            public const string UnitType = nameof(_unitType);
            public const string StatType = nameof(_statType);
            public const string StatBonus = nameof(_statBonus);
            public const string AttackDelayReductionPercent = nameof(_attackDelayReductionPercent);
            public const string StatCost = nameof(Cost);
            public const string StatCostInc = nameof(_statCostInc);
        }
#endif
    }
}

