using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using Managers.Loaders;
using turrets;
using Ui.Buttons.Deploy_Button;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Spawners
{
    public class EnemyTurretSlotSpawner : EnemySpawner<EnemyTurretSlotSpawner>
    {
        public System.Action<TurretSpawnPoint> OnTurretSlotActivated;
        protected override float RandomSpawnTimer { get; set ; }
        protected override float Timer { get; set; } = 0;

        private List<Lane> _lanes = new List<Lane>();

        public List<TurretSpawnPoint> TurretSpawnPoints { get; }  = new List<TurretSpawnPoint>();

        private bool _isSlotWaitingToBeAdded = false;

        protected override void Awake()
        {
            base.Awake();
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
        }

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _lanes.Clear();
            _lanes.AddRange(LaneManager.Instance.Lanes);
            SubscribeToAllLanes();
            _isSlotWaitingToBeAdded = true;
            EnemyTurretSpawner.Instance.OnTurretPlaced += StartTimer;
            GetAllEnemyTurretSpawnPoints();
            ResetTimer();
        }

        private void SubscribeToAllLanes()
        {
            foreach (var lane in _lanes)
            {
                lane.OnLaneDestroyed += RemoveDestroyedTurretSlots;
            }
        }

        private void OnDestroy()
        {
            foreach (var lane in _lanes)
            {
                lane.OnLaneDestroyed -= RemoveDestroyedTurretSlots;
            }
        }

        private void StartTimer()
        {
            ResetTimer();
            
            if (HasSlotToUnlock())
            {
                _isSlotWaitingToBeAdded = true;
            }

        }


        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu() || !GameStates.Instance.GameIsPlaying) return;
            if (!_isSlotWaitingToBeAdded)  return; 
            CheckIfSpawnable();
        }
        private void CheckIfSpawnable()
        {
            Timer += Time.deltaTime;
            if (CanDeploy())
            {
                SpawnRandomTurretSlot();
            }

        }


        private void SpawnRandomTurretSlot()
        {

            var AvailableSlot = GetAvailableTurretSlot();
            if (AvailableSlot != null)
            {
                AvailableSlot.IsUnlocked = true;
                AvailableSlot.ShowHighlight(true);

                OnTurretSlotActivated?.Invoke(AvailableSlot);

                ResetTimer();
                _isSlotWaitingToBeAdded = false;
            }
        }
        private void GetAllEnemyTurretSpawnPoints()
        {
            foreach (var lane in _lanes)
            {
                var turretSpawnPoint = lane.GetComponentsInChildren<TurretSpawnPoint>();
                TurretSpawnPoints.AddRange(turretSpawnPoint.
                    Where(spawnPoint => !spawnPoint.IsFriendly));
            }   
        }
        
        private void RemoveDestroyedTurretSlots(Lane lane)
        {
            if (lane == null) return;

            // Get all turret spawn points in the destroyed lane
            var destroyedSpawnPoints = lane.GetComponentsInChildren<TurretSpawnPoint>()
                .Where(spawnPoint => !spawnPoint.IsFriendly)
                .ToList();

            // Remove them from the global list
            foreach (var spawnPoint in destroyedSpawnPoints)
            {
                TurretSpawnPoints.Remove(spawnPoint);
            }

            
            _lanes.Remove(lane);
        }


        

        private TurretSpawnPoint GetAvailableTurretSlot()
        {
            return TurretSpawnPoints.FirstOrDefault(spawnPoint => !spawnPoint.IsUnlocked);
        }

        protected override bool CanDeploy()
        {
            return Timer >= RandomSpawnTimer;
        }

        private bool HasSlotToUnlock()
        {
            return TurretSpawnPoints.Any(spawnPoint => !spawnPoint.IsUnlocked);
        }

        private void ResetTimer()
        {
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
            Timer = 0;
        }

    }
}
