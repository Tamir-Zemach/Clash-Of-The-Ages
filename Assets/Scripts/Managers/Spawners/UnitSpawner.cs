using System;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Project_inspector_Addons;
using BackEnd.Structs;
using Bases;
using Managers.Loaders;
using Ui.Buttons.Deploy_Button;
using units.Behavior;
using UnityEngine;


namespace Managers.Spawners
{
    public class UnitSpawner : SceneAwareMonoBehaviour<UnitSpawner>
    {
        
        public static event Action<Lane, UnitBaseBehaviour> OnUnitDeployedOnLane;
        public static event Action OnMaxCapacity;
        

        [SerializeField, TagSelector] private string _spawnAreaTag;
        [SerializeField, TagSelector] private string _baseTag;
        public int MaxDeployableUnits;
        
        private bool _waitingForClearSpawn;
        private Deployment? _pendingDeployment;
        private Lane _defaultLane;

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            
            UnitDeploymentQueue.Instance.OnUnitReadyToDeploy += AttemptDeployment;
            
            if (EnemyBasesManager.Instance.MultipleBases()) return;
            
            _defaultLane = FindAnyObjectByType<Lane>();
            
        }
        


        private void AttemptDeployment(Deployment? deployment)
        {
            var lane = deployment?.Lane ?? _defaultLane;
            var unit = deployment?.Unit;
            if (unit == null) return;

            _pendingDeployment = new Deployment(unit, lane);
            _waitingForClearSpawn = true;
        }
        
        private void Update()
        {
            if (!_waitingForClearSpawn || _pendingDeployment == null) return;
            
            var lane = _pendingDeployment.Value.Lane ?? _defaultLane;
            if (!IsSpawnAreaInLaneClear(lane)) return;
            
            SpawnAndInitializeUnit(_pendingDeployment.Value.Unit, lane);
            _waitingForClearSpawn = false;
            _pendingDeployment = null;
            
        }

  

        
        
        private void SpawnAndInitializeUnit(UnitData unit, Lane lane)
        {
            var spawnPoint = lane.PlayerUnitSpawnPosition;
            var rotation = SetNormalizePos(lane); 
            var unitInstance = Instantiate(unit.Prefab, spawnPoint.position, rotation);

            if (!unitInstance.TryGetComponent(out UnitBaseBehaviour behaviour)) return;
            
            InitializeUnitBehaviour(behaviour, unit, lane);
                
            OnUnitDeployedOnLane?.Invoke(lane, behaviour);
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

        private Quaternion SetNormalizePos(Lane lane)
        {
            Vector3 direction = lane.EnemyBase.transform.position - lane.PlayerUnitSpawnPosition.position;
            direction.Normalize();
            return Quaternion.LookRotation(direction);
        }
        
        
        private bool IsSpawnAreaInLaneClear(Lane lane)
        {
            var spawnArea = lane.PlayerSpawnArea;
            return spawnArea != null && !spawnArea.HasUnitInside;
        }
        
        public bool MaxUnitCapacity()
        {
            if (GlobalUnitCounter.Instance.FriendlyCount + 1 <= MaxDeployableUnits) return false;
            OnMaxCapacity?.Invoke();
            return true;
        }
        
    }
}