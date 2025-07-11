using Assets.Scripts.Backend.Data;
using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using Assets.Scripts.turrets;
using Assets.Scripts.units;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Singleton Data")]
    [field: SerializeField] public SpecialAttackData FriendlySpecialAttack { get; private set; }
    [field: SerializeField] public SpecialAttackData EnemySpecialAttack { get; private set; }

    [Header("Level Up Data")]
    [field: SerializeField] public List<UnitLevelUpData> UnitLevelUpData { get; private set; }
    [field: SerializeField] public List<TurretLevelUpData> TurretLevelUpData { get; private set; }
    [field: SerializeField] public List<SpecialAttackLevelUpData> SpecialAttackLevelUpData { get; private set; }
    [field: SerializeField] public List<SpritesLevelUpData> SpritesLevelUpData { get; private set; }
    #endregion

    #region Unity Lifecycle

    protected override void Awake()
    {
        base.Awake();
        InstantiateAllData();
        OnInitialized?.Invoke();
    }

    #endregion

    #region Initialization

    private void InstantiateAllData()
    {
        InstantiateList(FriendlyUnits);
        InstantiateList(EnemyUnits);
        InstantiateList(FriendlyTurrets);
        InstantiateList(EnemyTurrets);
        InstantiateList(UnitLevelUpData);
        InstantiateList(TurretLevelUpData);
        InstantiateList(SpecialAttackLevelUpData);
        InstantiateList(SpritesLevelUpData);

        FriendlySpecialAttack = Instantiate(FriendlySpecialAttack);
        if (EnemySpecialAttack != null)
        {
            EnemySpecialAttack = Instantiate(EnemySpecialAttack);
        }
    }

    private void InstantiateList<T>(List<T> list) where T : UnityEngine.Object
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
                list[i] = Instantiate(list[i]);
        }
    }

    #endregion

}


public static class DataExtensions
{
    public static TData GetData<TType, TData>(this List<TData> list, TType type)
        where TData : class, IUpgradable<TType>
    {
        return list.FirstOrDefault(d => EqualityComparer<TType>.Default.Equals(d.Type, type));
    }
}