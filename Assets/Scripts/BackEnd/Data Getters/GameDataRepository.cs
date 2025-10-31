using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Enums;
using BackEnd.InterFaces;
using UnityEngine;

namespace BackEnd.Data_Getters
{
    public class GameDataRepository : SingletonMonoBehaviour<GameDataRepository>
    {
        public event Action OnInitialized;

        #region Serialized Source Data (Immutable)

        [Header("Units (Source Data)")]
        [SerializeField] private List<UnitData> friendlyUnitSources;
        [SerializeField] private List<UnitData> enemyUnitSources;

        [Header("Turrets (Source Data)")]
        [SerializeField] private List<TurretData> friendlyTurretSources;
        [SerializeField] private List<TurretData> enemyTurretSources;

        [Header("Special Attacks (Source Data)")]
        [SerializeField] private List<SpecialAttackData> friendlySpecialSources;
        [SerializeField] private List<SpecialAttackData> enemySpecialSources;

        [Header("Level Up Data (Immutable)")]
        [field: SerializeField] public List<LevelUpDataGroup> PlayerLevelUpData { get; private set; }
        [field: SerializeField] public List<LevelUpDataGroup> EnemyLevelUpData { get; private set; }

        #endregion

        #region Runtime Working Data (Mutable)

        public List<UnitData> FriendlyUnits { get; private set; } = new List<UnitData>();
        public List<UnitData> EnemyUnits { get; private set; } = new List<UnitData>();

        public List<TurretData> FriendlyTurrets { get; private set; } = new List<TurretData>();
        public List<TurretData> EnemyTurrets { get; private set; } = new List<TurretData>();

        public List<SpecialAttackData> FriendlySpecialAttacks { get; private set; } = new List<SpecialAttackData>();
        public List<SpecialAttackData> EnemySpecialAttacks { get; private set; } = new List<SpecialAttackData>();

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

        public void ResetData()
        {
            // Destroy old runtime instances
            DestroyList(FriendlyUnits);
            DestroyList(EnemyUnits);
            DestroyList(FriendlyTurrets);
            DestroyList(EnemyTurrets);
            DestroyList(FriendlySpecialAttacks);
            DestroyList(EnemySpecialAttacks);

            // Re-instantiate fresh runtime copies from immutable sources
            InstantiateAllData();

            OnInitialized?.Invoke();
        }

        private void DestroyList<T>(List<T> list) where T : UnityEngine.Object
        {
            foreach (var item in list)
            {
                if (item != null)
                    Destroy(item);
            }
            list.Clear();
        }

        private void InstantiateAllData()
        {
            FriendlyUnits = InstantiateList(friendlyUnitSources);
            EnemyUnits = InstantiateList(enemyUnitSources);

            FriendlyTurrets = InstantiateList(friendlyTurretSources);
            EnemyTurrets = InstantiateList(enemyTurretSources);

            FriendlySpecialAttacks = InstantiateList(friendlySpecialSources);
            EnemySpecialAttacks = InstantiateList(enemySpecialSources);
        }

        private List<T> InstantiateList<T>(List<T> source) where T : UnityEngine.Object
        {
            var newList = new List<T>(source.Count);
            foreach (var item in source)
            {
                if (item != null)
                    newList.Add(Instantiate(item));
            }
            return newList;
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
}