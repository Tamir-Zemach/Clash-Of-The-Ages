using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackButton : MonoBehaviour, IImgeSwichable<SpecialAttackType>
{
    [field: SerializeField] public SpecialAttackType Type {  get; private set; }

    [SerializeField] private int _cost;

    private Transform _specialAttackSpawnPos;

    private SpecialAttackData _specialAttack;

    private Image _image;


    private void Start()
    {
        GetData();
    }

    private void GetData()
    {
        _specialAttack = GameDataRepository.Instance.FriendlySpecialAttacks.GetData(Type);
        _image = GetComponent<Image>();
        UIRootManager.Instance.OnSceneChanged += GetSpawnPos;
        GameManager.Instance.OnAgeUpgrade += UpdateSprite;
        GetSpawnPos(); 
    }

    private void GetSpawnPos()
    { 
        _specialAttackSpawnPos = GameObject.FindGameObjectWithTag(_specialAttack.SpawnPosTag).transform;
    }

    public void UpdateSprite(List<LevelUpDataBase> upgradeDataList)
    {
        foreach (var data in upgradeDataList)
        {
            if (data is SpritesLevelUpData levelUpData)
            {
                _image.sprite = levelUpData.GetSpriteFromList(Type, levelUpData.SpecialAttackSpriteMap);
            }
        }
    }

    public void PerformSpecialAttack()
    {
        if (PlayerCurrency.Instance.HasEnoughMoney(_cost) && !MeteorRainAlreadyExists())
        {
            PlayerCurrency.Instance.SubtractMoney(_cost);
            ApplySpecialAttack();
        }
    }

    private void ApplySpecialAttack()
    {

        switch (_specialAttack.AgeStage)
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
                Debug.LogWarning("Unknown AgeStageType type: " + _specialAttack.AgeStage);
                break;
        }
    }

    private bool MeteorRainAlreadyExists()
    {
        return FindAnyObjectByType<MeteorRain>() != null;
    }

}
