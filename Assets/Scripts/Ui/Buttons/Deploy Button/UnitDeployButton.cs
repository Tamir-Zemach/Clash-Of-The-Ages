using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Economy;
using BackEnd.InterFaces;
using Managers;
using Managers.Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Buttons.Deploy_Button
{
    public class UnitDeployButton : ButtonWithCost, IImageSwitchable<UnitType>
    {
        [SerializeField] private UnitType _unitType;

        private UnitData _unit;

        private Image _image;
        
        public UnitType Type => _unitType;
        
        private void Awake()
        {
            _unit = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
            _image = GetComponent<Image>();
            GameManager.Instance.OnAgeUpgrade += UpdateSprite;
        }

        private void UpdateSprite(List<LevelUpDataBase> upgradeDataList)
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

            if (PlayerCurrency.Instance.HasEnoughMoney(Cost))
            {
                PlayerCurrency.Instance.SubtractMoney(Cost);

                if (GameManager.Instance != null && DeployManager.Instance != null)
                {
                    DeployManager.Instance.AddUnitToDeploymentQueue(_unit);
                }
            }
        }
    }
}
