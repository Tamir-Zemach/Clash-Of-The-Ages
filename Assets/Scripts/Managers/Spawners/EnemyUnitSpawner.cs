using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Project_inspector_Addons;
using Bases;
using Ui.Buttons.Deploy_Button;
using units.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Spawners
{
    //TODO: Enemy doesnt go toward base
    public class EnemyUnitSpawner : EnemySpawner<EnemyUnitSpawner>
    {
        [Tooltip("Maximum number of enemies allowed to spawn.")]
        [SerializeField] private int _maxEnemies;

        private FriendlyBaseHealth _playerBase;
        private readonly List<SpawnArea> _enemySpawnAreas =  new List<SpawnArea>();
        private readonly List<Transform> _enemySpawnPoints  = new List<Transform>();
        private List<UnitData> _enemyUnits  = new List<UnitData>();
        private readonly List<Lane> _lanes = new List<Lane>();
        
        protected override float RandomSpawnTimer { get; set; }
        protected override float Timer { get; set ; }

        protected override void Awake()
        {
            base.Awake();
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
        }
        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _lanes.AddRange(LaneManager.Instance.Lanes);
            GetEnemySpawnPoints();
            _enemyUnits = GameDataRepository.Instance.EnemyUnits;
            _playerBase = FindAnyObjectByType<FriendlyBaseHealth>();
            
        }
        
        private void GetEnemySpawnPoints()
        {
            _enemySpawnPoints.Clear();
            _enemySpawnAreas.Clear();
            foreach (var lane in _lanes)
            {
                _enemySpawnPoints.Add(lane.EnemyUnitSpawnPosition);
                _enemySpawnAreas.Add(lane.EnemySpawnArea);
            }
        }


        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu() || !GameStates.Instance.GameIsPlaying) return;
            SpawnPrefabsWithRandomTime();

        }

        private void SpawnPrefabsWithRandomTime()
        {
            Timer += Time.deltaTime;
            if (CanDeploy())
            {
                SpawnRandomEnemyPrefab();
                Timer = 0;
                RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
            }
        }

        private void SpawnRandomEnemyPrefab()
        {
            if (_enemyUnits == null || _enemyUnits.Count == 0) return;

            Lane chosenLane = GetAvailableLane();
            if (chosenLane == null) return;

            UnitData randomEnemyData = _enemyUnits[Random.Range(0, _enemyUnits.Count)];
            InstantiateAndInitializeUnit(randomEnemyData, chosenLane.EnemyUnitSpawnPosition, _playerBase.transform);
        }
        
        private void InstantiateAndInitializeUnit(UnitData unitData, Transform spawnPoint, Transform destination)
        {
            GameObject enemyReference = Instantiate(unitData.Prefab, spawnPoint.position, RotateTowardsBase(spawnPoint.position));
            if (enemyReference.TryGetComponent(out UnitBaseBehaviour behaviour))
            {
                behaviour.Initialize(unitData, destination);
            }
        }

        protected override bool CanDeploy()
        {
            return Timer >= RandomSpawnTimer && GetAvailableLane() != null && UnitCounter.EnemyCount < _maxEnemies;
        }
        
        
        private Lane GetAvailableLane()
        {
            var availableLanes = _lanes.Where(lane =>
                !lane.EnemySpawnArea.HasUnitInside &&
                !lane.IsDestroyed).ToList();
            if (availableLanes.Count == 0) return null;

            return availableLanes[Random.Range(0, availableLanes.Count)];
        }
        
        
        private Quaternion RotateTowardsBase(Vector3 spawnPosition)
        {
            Vector3 directionToBase = (_playerBase.transform.position - spawnPosition).normalized;
            return Quaternion.LookRotation(directionToBase);
        }

    }
}