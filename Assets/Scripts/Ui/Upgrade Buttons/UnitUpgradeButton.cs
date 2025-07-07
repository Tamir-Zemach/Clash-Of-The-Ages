using Assets.Scripts;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;
using UnityEngine.UI;


public class UnitUpgradeButton : MonoBehaviour, IImgeSwichable<UnitType>
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

    [Tooltip("Cost to upgrade unit stat")]
    [SerializeField] private int _statCost;

    [Tooltip("Incremental increase in stat upgrade cost after each upgrade")]
    [SerializeField] private int _statCostInc;

    private Sprite _sprite;
    private Image _image;
   

    public UnitType Type => _unitType;
    public StatType StatType => _statType;
    public Sprite Sprite => _sprite;
    public Image Image => _image;
    public int Cost => _statCost;

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
    private void Start()
    {
        GetData();
    }

    private void GetData()
    {
        _sprite = GameStateManager.Instance.GetUnitUpgradebuttonSpritesSprite((_unitType, _statType));
        _image = GetComponent<Image>();
        _image.sprite = _sprite;
    }

    public void UpgradeStat()
    {
        var unit = GameStateManager.Instance.GetFriendlyUnit(_unitType);

        if (PlayerCurrency.Instance.HasEnoughMoney(_statCost))
        {
            PlayerCurrency.Instance.SubtractMoney(_statCost);
            ApplyUpgrade(unit);
            _statCost += _statCostInc;
            GameStateManager.Instance.SetStatUpgradeCost(_unitType, _statType, _statCost);
        }
    }

    private void ApplyUpgrade(UnitData unit)
    {
        switch (_statType)
        {
            case StatType.Strength:
                unit.Strength += _statBonus;
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
        public const string StatCost = nameof(_statCost);
        public const string StatCostInc = nameof(_statCostInc);
    }
#endif
}

