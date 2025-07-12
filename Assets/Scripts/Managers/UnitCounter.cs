using UnityEngine;

public class UnitCounter : MonoBehaviour
{
    private UnitBaseBehaviour unitBaseBehaviour;

    public static int FriendlyCount { get; private set; }
    public static int EnemyCount { get; private set; }

    private void Awake()
    {
        unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
    }
    private void Start()
    {
        if (unitBaseBehaviour.Unit.IsFriendly)
            FriendlyCount++;
        else
            EnemyCount++;
    }

    private void OnDestroy()
    {
        if (unitBaseBehaviour != null && unitBaseBehaviour.Unit != null)
        {
            if (unitBaseBehaviour.Unit.IsFriendly)
                FriendlyCount--;
            else
                EnemyCount--;
        }
    }
}