
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using Bases;
using Managers.Loaders;
using Ui.Buttons.Deploy_Button;
using units.Behavior;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Spawners
{
    //TODO: Fix Bug on resets
    public class EnemyUnitSpawner : EnemySpawner<EnemyUnitSpawner>
    {
        [Tooltip("Maximum number of enemies allowed to spawn.")]
        [SerializeField] private int _maxEnemies;

        private FriendlyBaseHealth _playerBase;
        private readonly List<SpawnArea> _enemySpawnAreas = new List<SpawnArea>();
        private readonly List<Transform> _enemySpawnPoints = new List<Transform>();
        private List<UnitData> _enemyUnits = new List<UnitData>();
        private readonly List<Lane> _lanes = new List<Lane>();

        protected override float RandomSpawnTimer { get; set; }
        protected override float Timer { get; set; }

        protected override void Awake()
        {
            base.Awake();
            ResetAndReRandomTimer();
        }

        protected override void InitializeOnSceneLoad()
        {

            if (LevelLoader.Instance.InStartMenu()) return;

            _lanes.Clear();
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

            Timer += Time.deltaTime;

            if (Timer >= RandomSpawnTimer)
            {

                if (!CanDeploy())
                {
                    Timer = RandomSpawnTimer;
                    return;
                }

                Lane chosenLane = GetAvailableLane();
                if (chosenLane == null)
                {
                    Timer = RandomSpawnTimer;
                    return;
                }

                SpawnRandomEnemyPrefab(chosenLane);
                ResetAndReRandomTimer();
            }
        }

        private void SpawnRandomEnemyPrefab(Lane chosenLane)
        {
            if (_enemyUnits == null || _enemyUnits.Count == 0) return;
            
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
            return Timer >= RandomSpawnTimer && GlobalUnitCounter.Instance.EnemyCount < _maxEnemies;
        }

        private Lane GetAvailableLane()
        {
            var availableLanes = _lanes
                .Where(lane => !lane.EnemySpawnArea.HasUnitInside && !lane.IsDestroyed)
                .ToList();
            
            return availableLanes.Count == 0
                ? null
                : availableLanes[Random.Range(0, availableLanes.Count)];
        }

        private Quaternion RotateTowardsBase(Vector3 spawnPosition)
        {
            Vector3 directionToBase = (_playerBase.transform.position - spawnPosition).normalized;
            return Quaternion.LookRotation(directionToBase);
        }

        public override void HandlePause() { }

        public override void HandleResume() { }

        public override void HandleGameEnd()
        { 
            ResetAndReRandomTimer();
        }
    }
}