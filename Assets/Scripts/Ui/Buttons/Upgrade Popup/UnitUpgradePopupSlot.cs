using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UnitUpgradePopupSlot : MonoBehaviour
    {
        [Tooltip("Which unit should be upgraded")]
        [SerializeField] private UnitType _unitType;

        [Tooltip("What Stat to Upgrade")] 
        [SerializeField] private StatType _stat;
        
        [Header("Upgrade stats Settings")]
        [Tooltip("Stat bonus added To the Unit")]
        [SerializeField] private int _statBonus;
        
        [Header("Upgrade stats Settings")]
        [Tooltip("Percentage to reduce attack delay per upgrade (i.e., increase attack speed).")]
        [SerializeField] private float _attackDelayReductionPercent;

        private UnitData _unit;
        private void Start()
        {
            GetData();
        }

        private void GetData()
        {
            if (GameDataRepository.Instance == null) return;
            _unit = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
        }
        
        public void ApplyUpgrade()
        {
            switch (_stat)
            {
                case StatType.Strength:
                    _unit.MinStrength += _statBonus;
                    _unit.MaxStrength += _statBonus;
                    break;

                case StatType.Health:
                    _unit.Health += _statBonus;
                    break;

                case StatType.Range:
                    _unit.Range += _statBonus;
                    break;

                case StatType.AttackSpeed:
                    _unit.InitialAttackDelay *= 1f - (_attackDelayReductionPercent / 100f);
                    break;

                default:
                    Debug.LogWarning("Unknown stat type: " + _stat);
                    return; // Exit early to avoid hiding popup on unknown stat
            }
            
            UpgradePopup.Instance.BlockRaycasts(false);

            UIEffects.ShrinkAndDestroy(transform, 1.2f, 0, () =>
            {
                UpgradePopup.Instance.HidePopup();
            });
            
        }
#if UNITY_EDITOR
        public static class FieldNames
        {
            public const string UnitType = nameof(_unitType);
            public const string StatType = nameof(_stat);
            public const string StatBonus = nameof(_statBonus);
            public const string AttackDelayReductionPercent = nameof(_attackDelayReductionPercent);
        }
#endif
        
    }
}
