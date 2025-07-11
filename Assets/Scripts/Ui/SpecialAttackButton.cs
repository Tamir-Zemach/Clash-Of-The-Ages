using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackButton : MonoBehaviour, IImgeSwichable<AgeStageType>
{
    [SerializeField] private AgeStageType _ageStageType;

    private Transform _specialAttackSpawnPos;

    private SpecialAttackData _specialAttack;

    private Image _image;

    public AgeStageType Type => _ageStageType;

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
        _specialAttack = GameDataRepository.Instance.FriendlySpecialAttack;
        _image = GetComponent<Image>();
        UIRootManager.Instance.OnSceneChanged += GetSpawnPos;
        GetSpawnPos(); 
    }

    private void GetSpawnPos()
    { 
        _specialAttackSpawnPos = GameObject.FindGameObjectWithTag(_specialAttack.SpawnPosTag).transform;
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
        switch (_ageStageType)
        {
            case AgeStageType.StoneAge:
                Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.position, _specialAttackSpawnPos.rotation);
                break;
            case AgeStageType.Military:
                //same logic for now
                Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.position, _specialAttackSpawnPos.rotation);
                break;
            case AgeStageType.Future:
                //same logic for now
                Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.position, _specialAttackSpawnPos.rotation);
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
