using System.Linq;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Data__ScriptableOBj_;
using Managers.Spawners;
using TMPro;
using UnityEngine;

namespace Ui.Buttons.Deploy_Button
{
    public class UnitQueueUiHandler : MonoBehaviour
    {

        [Tooltip("Text to dipsplay the queue index, needs to be child of this GameObject")]
        [SerializeField] private TextMeshProUGUI _queueCountText;

        [SerializeField] private UnitType _unitType;


        private UnitData[] _queueArray;
        private UnitData _assignedUnit;

        public UnitType UnitType => _unitType;



        private void Awake()
        {
            _queueCountText = GetComponentInChildren<TextMeshProUGUI>();
            DeployManager.OnQueueChanged += UpdateQueueIndex;
        }
        private void Start()
        {
            SetAssignedUnit(GameDataRepository.Instance.FriendlyUnits.GetData(_unitType));
        }
        private void OnDestroy()
        {
            DeployManager.OnQueueChanged -= UpdateQueueIndex;
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
            _queueArray = DeployManager.Instance.UnitQueue.Where(c => c == _assignedUnit).ToArray();

            _queueCountText.text = _queueArray.Length > 0 ? $"+ {_queueArray.Length}" : "";
        }

        private bool NullChecksForSafety()
        {
            if (_assignedUnit == null ||
                DeployManager.Instance == null || DeployManager.Instance.UnitQueue == null)
            {
                _queueCountText.text = "Error: Queue not initialized";
                return true;
            }
            else { return false; }
        }


    }
}



