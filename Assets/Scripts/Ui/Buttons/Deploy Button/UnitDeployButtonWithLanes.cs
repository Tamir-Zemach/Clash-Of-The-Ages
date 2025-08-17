
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
    public class UnitDeployButtonWithLanes : ButtonWithCost, IImageSwitchable<UnitType>
    {
        
        [SerializeField] private UnitType _unitType;

        private UnitData _unit;

        private Image _image;
        
        public UnitType Type => _unitType;

        private bool _choseValidLane;
        
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
            if (_unit == null)
            {
                Debug.LogWarning($"[UnitDeployButton] Unit data not initialized for {_unitType}");
                return;
            }

            if (CanDeployUnit())
            {

                if (EnemyBasesManager.Instance.MultipleBases())
                {
                    ChooseLane(() =>
                    {
                        if (_choseValidLane)
                        {
                            PlayerCurrency.Instance.SubtractMoney(Cost);
                            if (GameManager.Instance != null && DeployManager.Instance != null)
                            {
                                DeployManager.Instance.QueueUnitForDeployment(_unit, _selectedLane);
                            }
                        }
                    });
                }
                else
                {
                    PlayerCurrency.Instance.SubtractMoney(Cost);

                    if (GameManager.Instance != null && DeployManager.Instance != null)
                    {
                        DeployManager.Instance.QueueUnitForDeployment(_unit);
                    } 
                }
            }
        }
        

        private void ChooseLane(Action onLaneChosen)
        {
            MouseRayCaster.Instance.StartClickRoutine(
                onValidHit: hit =>
                {
                    if (hit.collider.GetComponentInParent<Lane>() is Lane lane && !lane.IsDestroyed)
                    {
                        _choseValidLane = true;
                        _selectedLane = lane;
                        onLaneChosen?.Invoke();
                    }
                    else
                    {
                        _choseValidLane = false;
                    }
                },
                onMissedClick: () =>
                {
                    _choseValidLane = false;
                });
            
        }

        private bool CanDeployUnit()
        {
            return PlayerCurrency.Instance.HasEnoughMoney(Cost) && !DeployManager.Instance.MaxUnitCapacity();
        }
        
    }
}