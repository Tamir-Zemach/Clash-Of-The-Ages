using System;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Project_inspector_Addons;
using Bases;
using Ui.Buttons.Deploy_Button;
using units.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.Spawners
{
    public class DeployManager : SceneAwareMonoBehaviour<DeployManager>
    {
        #region Events

        public static event Action OnUnitDeployed;
        public static event Action<Lane, UnitBaseBehaviour> OnUnitDeployedOnLane;
        public static event Action OnMaxCapacity;

        #endregion

        #region  Fields & Initialization

        [SerializeField, TagSelector] private string _spawnAreaTag;
        [SerializeField, TagSelector] private string _baseTag;
        public int MaxDeployableUnits;
        
        private Lane _defaultLane;
        private GameObject _unitInstance;
        private float _timer;

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;

            UnitDeploymentQueue.Instance.ClearAllDeploymentQueues();
            
            UnitDeploymentQueue.Instance.OnUnitReadyToDeploy += ResetTimer;
            
            if (EnemyBasesManager.Instance.MultipleBases()) return;
            
            _defaultLane = FindAnyObjectByType<Lane>();

        }

        private void OnDestroy()
        {
            UnitDeploymentQueue.Instance.OnUnitReadyToDeploy -= ResetTimer;
        }

        private void ResetTimer(UnitData obj)
        {
            _timer = 0;
        }

        #endregion

        #region Deployment Logic

        
        private void Update()
        {
            if (!GameStates.Instance.GameIsPlaying) return;
            
            _timer += Time.deltaTime;
            TryDeployNextUnit();
        }

        
        /// <summary>
        /// Checks the deployment queue and attempts to deploy the next unit if ready.
        /// </summary>
        private void TryDeployNextUnit()
        {
            var nextUnit = UnitDeploymentQueue.Instance.GetNextUnit();
            if (nextUnit.HasValue)
            {
                AttemptDeployment(nextUnit.Value.unit, nextUnit.Value.lane);
            }
        }
        
        
        /// <summary>
        /// Adds a unit to the deployment queue.
        /// </summary>
        public void QueueUnitForDeployment(UnitData unit, Lane lane = null)
        {
            UnitDeploymentQueue.Instance.EnqueueUnit(unit, lane);
            
        }
        
        private void AttemptDeployment(UnitData unit, Lane lane = null)
        {
            if (!CanDeploy(unit, lane)) return;
            
            SpawnAndInitializeUnit(unit, lane);
            FinishDeployment();
        }

        /// <summary>
        /// Finalizes deployment by triggering events and resetting the timer.
        /// </summary>
        private void FinishDeployment()
        {
            OnUnitDeployed?.Invoke();
            _timer = 0;
            UnitDeploymentQueue.Instance.CompleteCurrentDeployment();
        }
        
        
        private void SpawnAndInitializeUnit(UnitData unit, Lane lane = null)
        {
            // Use lane-specific spawn point if provided; otherwise, use the default lane's spawn point (single-lane fallback).
            var spawnPoint = lane != null ? lane.PlayerUnitSpawnPosition : _defaultLane.PlayerUnitSpawnPosition;

            _unitInstance = Instantiate(unit.Prefab, spawnPoint.position, spawnPoint.rotation);

            if (_unitInstance.TryGetComponent(out UnitBaseBehaviour behaviour))
            {
                InitializeUnitBehaviour(behaviour, unit, lane);
                //print("unit deployed");
                OnUnitDeployedOnLane?.Invoke(lane, behaviour);
            }
        }
        
        private void InitializeUnitBehaviour(UnitBaseBehaviour behaviour, UnitData unit, Lane lane = null)
        {
            if (lane == null)
            {
                behaviour.Initialize(unit);
            }
            else
            {
                behaviour.Initialize(unit, lane.EnemyBase);
            }
            
            
        }
        #endregion
        
        #region Deployment Checks
        /// <summary>
        /// Checks if the unit can be deployed based on spawn area availability and delay timer.
        /// </summary>
        private bool CanDeploy(UnitData unit, Lane lane = null)
        {
            var targetLane = lane ?? _defaultLane;
            var spawnArea = targetLane.PlayerSpawnArea;

            if (spawnArea == null || spawnArea.HasUnitInside)
            {
                return false;
            }
            
            return _timer >= unit.DeployDelayTime;
        }
        
        public bool MaxUnitCapacity()
        {
            if (UnitCounter.FriendlyCount + UnitDeploymentQueue.Instance.CurrentUnitsInQueue >= MaxDeployableUnits)
            {
                OnMaxCapacity?.Invoke();
                return true;
            }
            return false;
        }

        #endregion
    }
}