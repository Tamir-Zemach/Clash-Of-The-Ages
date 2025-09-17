using System.Linq;
using BackEnd.Enums;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using Managers.Spawners;
using TMPro;
using UnityEngine;

namespace Ui.Buttons.Deploy_Button
{
    public class UnitQueueUiHandler : MonoBehaviour
    {

        [Tooltip("Text to display the queue index, needs to be child of this GameObject")]
        [SerializeField] private TextMeshProUGUI _queueCountText;

        [SerializeField] private UnitType _unitType;


        private UnitData[] _queueArray;
        private UnitData _assignedUnit;

        public UnitType UnitType => _unitType;
        
        private void Start()
        {
            UnitDeploymentQueue.Instance.OnQueueChanged += UpdateQueueIndex;
            SetAssignedUnit(GameDataRepository.Instance.FriendlyUnits.GetData(_unitType));
        }
        private void OnDestroy()
        {
            UnitDeploymentQueue.Instance.OnQueueChanged -= UpdateQueueIndex;
        }
        public void SetAssignedUnit(UnitData unit)
        {
            _assignedUnit = unit;
            UpdateQueueIndex(); 
        }

        public void UpdateQueueIndex()
        {
            if (NullChecksForSafety())
            {
                return;
            }
            // Filter queue to only include characters matching _assignedCharacter
            _queueArray = UnitDeploymentQueue.Instance.UnitQueue
                .Where(unit => unit.Type == _assignedUnit.Type)
                .ToArray();

            _queueCountText.text = _queueArray.Length > 0 ? $"+ {_queueArray.Length}" : "";
        }

        private bool NullChecksForSafety()
        {
            if (_assignedUnit == null || UnitSpawner.Instance == null)
            {
                _queueCountText.text = "Error: Queue not initialized";
                return true;
            }
            return false;
        }


    }
}



