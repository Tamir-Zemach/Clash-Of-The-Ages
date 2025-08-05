
using Assets.Scripts.Data;
using BackEnd.Enums;
using Assets.Scripts.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using UnityEngine;

public class GameDataRepository : PersistentMonoBehaviour<GameDataRepository>
{
    public event Action OnInitialized;

    #region Serialized Fields

    [Header("Units")]
    [field: SerializeField] public List<UnitData> FriendlyUnits { get; private set; }
    [field: SerializeField] public List<UnitData> EnemyUnits { get; private set; }

    [Header("Turrets")]
    [field: SerializeField] public List<TurretData> FriendlyTurrets { get; private set; }
    [field: SerializeField] public List<TurretData> EnemyTurrets { get; private set; }

    [Header("Special Attacks")]
    [field: SerializeField] public List<SpecialAttackData> FriendlySpecialAttacks { get; private set; }
    [field: SerializeField] public List<SpecialAttackData> EnemySpecialAttacks { get; private set; }

    [Header("Level Up Data")]
    [field: SerializeField] public List<LevelUpDataGroup> PlayerLevelUpData { get; private set; }
    [field: SerializeField] public List<LevelUpDataGroup> EnemyLevelUpData { get; private set; }

    #endregion

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        InstantiateAllData();
    }
    private void Start()
    {
        OnInitialized?.Invoke();
    }
    private void InstantiateAllData()
    {
        InstantiateList(FriendlyUnits);
        InstantiateList(FriendlyTurrets);
        InstantiateList(FriendlySpecialAttacks);


        InstantiateList(EnemyUnits);
        InstantiateList(EnemyTurrets);
        InstantiateList(EnemySpecialAttacks);

    }

    private void InstantiateList<T>(List<T> list) where T : UnityEngine.Object
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                list[i] = Instantiate(list[i]);
            }

        }
    }

    #endregion

}
#region Extensions

public static class DataExtensions
{
    public static TData GetData<TType, TData>(this List<TData> list, TType type)
        where TData : class, IUpgradable<TType>
    {
        return list.FirstOrDefault(d => EqualityComparer<TType>.Default.Equals(d.Type, type));
    }
    public static TData GetLevelUpData<TType, TData>(
    this List<LevelUpDataGroup> groups,
    AgeStageType ageStage,
    TType type
)
    where TData : class, IUpgradable<TType>
    {
        var group = groups.FirstOrDefault(g => g.AgeStage == ageStage);
        return group?.LevelUpEntries.OfType<TData>().ToList().GetData(type);
    }
}

[System.Serializable]
public class LevelUpDataGroup
{
    public AgeStageType AgeStage;
    public List<LevelUpDataBase> LevelUpEntries;
}

#endregion