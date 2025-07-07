using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackButton : MonoBehaviour ,IImgeSwichable<AgeStageType>
{
    [SerializeField] private AgeStageType _ageStageType;

    [SerializeField] private Transform _meteorRainSpawnPos;

    private SpecialAttackData _specialAttack;

    private Sprite _sprite;

    private Image _image;

    public AgeStageType Type => _ageStageType;
    public Sprite Sprite => _sprite;
    public Image Image => _image;

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
        _specialAttack = GameStateManager.Instance.GetFriendlySpecialAttackData();
        _sprite = GameStateManager.Instance.GetSpecialAttackSprite(_ageStageType);
        _image = GetComponent<Image>();
        _image.sprite = _sprite;
    }





    public void PerformSpecialAttack()
    {
        if (PlayerCurrency.Instance.HasEnoughMoney(_specialAttack.Cost) && !MeteorRainAlreadyExists())
        {
            PlayerCurrency.Instance.SubtractMoney(_specialAttack.Cost);
            ApplySpecialAttack();
        }
    }

    private void ApplySpecialAttack()
    {
        var currentAttackPrfab = GameStateManager.Instance.GetSpecialAttackPrefab();
        switch (_ageStageType)
        {
            case AgeStageType.StoneAge:
                Instantiate(currentAttackPrfab, _meteorRainSpawnPos.position, _meteorRainSpawnPos.rotation);
                break;
            case AgeStageType.Military:
                //same logic for now
                Instantiate(currentAttackPrfab, _meteorRainSpawnPos.position, _meteorRainSpawnPos.rotation);
                break;
            case AgeStageType.Future:
                //same logic for now
                Instantiate(currentAttackPrfab, _meteorRainSpawnPos.position, _meteorRainSpawnPos.rotation);
                break;

            default:
                Debug.LogWarning("Unknown AgeStageType type: " + _ageStageType);
                break;
        }
    }

    private bool MeteorRainAlreadyExists()
    {
        return FindAnyObjectByType<MeteorRain>() != null;
    }

}
