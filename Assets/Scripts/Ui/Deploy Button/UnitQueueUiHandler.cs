
using Assets.Scripts.Enems;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
        SetAssignedUnit(GameStateManager.Instance.GetFriendlyUnit(_unitType));
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
        _queueArray = DeployManager.Instance._unitQueue.Where(c => c == _assignedUnit).ToArray();

        _queueCountText.text = _queueArray.Length > 0 ? $"+ {_queueArray.Length}" : "";
    }

    private bool NullChecksForSafety()
    {
        if (_assignedUnit == null ||
            DeployManager.Instance == null || DeployManager.Instance._unitQueue == null)
        {
            _queueCountText.text = "Error: Queue not initialized";
            return true;
        }
        else { return false; }
    }


}



