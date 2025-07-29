using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using Managers;
using Managers.Spawners;
using UnityEngine;
using UnityEngine.UI;
using static BackEnd.Utilities.SpriteKeys;

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
            UiAgeUpgrade.Instance.OnUiRefreshDeployUnits += UpdateSprite;
        }


        private void UpdateSprite(List<SpriteEntries.SpriteEntry<UnitType>> spriteMap)
        {
            foreach (var s in spriteMap)
            {
                if (s.GetKey() == _unitType)
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
