
using System;
using System.Collections;
using BackEnd.Enums;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using Bases;
using Managers;
using Managers.Spawners;
using Ui.Buttons.Upgrade_Popup;
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
        
        public UnitData UnitData => _unit;
        
        private Lane _selectedLane;
        
        
        private void Awake()
        {
            _unit = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
            _image = GetComponent<Image>();

            UnitAgeUpgradePopupSlot.OnUnitAgeUpgrade += UpdateSpriteFromSlot;
        }

        
        private void UpdateSpriteFromSlot(UnitType unitType, Sprite sprite)
        {
            if (unitType == _unitType)
            {
                _image.sprite = sprite;
            }
        }
        

        public void DeployUnit()
        {
            if (_unit == null || !CanDeployUnit()) return;
            
            if (EnemyBasesManager.Instance.MultipleBases())
            {
                LaneChooser.ChooseLane(
                    onLaneChosen: lane =>
                    {
                        ExecuteDeployment(lane);
                    },
                    onCancel: () =>
                    {
                    });
            }
            else
            {
                ExecuteDeployment();
            }
        }

        private void ExecuteDeployment(Lane lane = null)
        {
            PlayerCurrency.Instance.SubtractMoney(Cost);
            if (GameManager.Instance == null || UnitSpawner.Instance == null) return;
            
            GlobalUnitCounter.Instance.AddCount(1, friendly: true);
            UnitDeploymentQueue.Instance.EnqueueUnit(_unit, lane);
        }

        

        private bool CanDeployUnit()
        {
            return PlayerCurrency.Instance.HasEnoughMoney(Cost) && !UnitSpawner.Instance.MaxUnitCapacity();
        }
        
        
        
    }
}