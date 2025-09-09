
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Enums;
using TMPro;
using Ui.Buttons.Deploy_Button;
using UnityEngine;

namespace Ui
{
    public class DeployButtonPropertiesUpdater : MonoBehaviour
    {
        public TextMeshProUGUI CostText;
        public TextMeshProUGUI PowerText;

        private UnitDeployButtonWithLanes _deployButton;
        private UnitData  _unitData;
        private UnitType _unitType;
        private int _lastCost;
        private string _lastPowerText;
        

        private void Awake()
        {
            _deployButton = GetComponentInParent<UnitDeployButtonWithLanes>();
            _unitType  = _deployButton.Type;
            _unitData = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
            
        }

        private void Update()
        {
            if (_deployButton == null) return;

            // Update cost
            int currentCost = _deployButton.Cost;
            if (_lastCost != currentCost)
            {
                CostText.text = currentCost.ToString();
                _lastCost = currentCost;
            }

            // Add a public getter for _unit
            if (_unitData != null)
            {
                string currentPowerText = $"{_unitData.MinStrength} - {_unitData.MaxStrength}";
                if (_lastPowerText != currentPowerText)
                {
                    PowerText.text = currentPowerText;
                    _lastPowerText = currentPowerText;
                }
            }
        }
    }
}