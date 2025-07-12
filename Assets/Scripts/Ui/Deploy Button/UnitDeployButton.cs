using Assets.Scripts;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDeployButton : MonoBehaviour, IImgeSwichable<UnitType>
{
    [SerializeField] private UnitType _unitType;

    private UnitData _unit;

    private Image _image;


    public UnitType Type => _unitType;
    public void SetSprite(Sprite sprite)
    {
       _image.sprite = sprite;
    }
    private void Awake()
    {
        _unit = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
        _image = GetComponent<Image>();
        GameManager.Instance.OnAgeUpgrade += UpdateSprite;
    }
    public void UpdateSprite(List<LevelUpDataBase> upgradeDataList)
    {
        foreach (var data in upgradeDataList)
        {
            if (data is SpritesLevelUpData levelUpData)
            {
                _image.sprite = levelUpData.GetSpriteFromList(_unitType, levelUpData.UnitSpriteMap);
            }
        }
    }

    public void DeployUnit()
    {
        if (_unit == null)
        {
            Debug.LogWarning($"[UnitDeployButton] Unit data not initialized for {_unitType}");
            return;
        }

        if (PlayerCurrency.Instance.HasEnoughMoney(_unit.Cost))
        {
            PlayerCurrency.Instance.SubtractMoney(_unit.Cost);

            if (GameManager.Instance != null && DeployManager.Instance != null)
            {
                DeployManager.Instance.AddUnitToDeploymentQueue(_unit);
            }
        }
    }
}
