using Assets.Scripts.BackEnd.Enems;
using BackEnd.Data__ScriptableOBj_;
using UnityEngine;

namespace Ui.Buttons
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
            _unit = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
        }
        
        private void ApplyUpgrade(UnitData unit)
        {
            switch (_stat)
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
                    Debug.LogWarning("Unknown stat type: " + _stat);
                    break;
            }
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
