using System;
using System.Collections.Generic;
using BackEnd.Enums;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using Managers;
using Managers.Spawners;
using Ui.Buttons.Upgrade_Popup;
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

            UnitAgeUpgradePopupSlot.OnUnitAgeUpgrade += UpdateSpriteFromSlot;
        }

        
        //this is the name for now 
        private void UpdateSpriteFromSlot(UnitType unitType, Sprite sprite)
        {
            if (unitType == _unitType)
            {
                _image.sprite = sprite;
            }
        }

        

        public void DeployUnit()
        {
            if (_unit == null)
            {
                Debug.LogWarning($"[UnitDeployButton] Unit data not initialized for {_unitType}");
                return;
            }

            if (CanDeployUnit())
            {
                
                PlayerCurrency.Instance.SubtractMoney(Cost);

                if (GameManager.Instance != null && DeployManager.Instance != null)
                {
                    DeployManager.Instance.QueueUnitForDeployment(_unit);
                }
            }
        }

        private bool CanDeployUnit()
        {
            return PlayerCurrency.Instance.HasEnoughMoney(Cost) && !DeployManager.Instance.MaxUnitCapacity();
        }
        
    }
}
