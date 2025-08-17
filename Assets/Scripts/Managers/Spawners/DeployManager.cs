using System;
using System.Collections.Generic;
using System.Linq;
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
        public static event Action OnQueueChanged;
        
        public static event Action<UnitData, int> OnUnitQueued;
        public static event Action<UnitData> OnUnitReadyToDeploy;
        public static event Action OnUnitDeployed;
        public static event Action<Lane, UnitBaseBehaviour> OnUnitDeployedOnLane;
        public static event Action OnMaxCapacity;

        public readonly Queue<UnitData> UnitQueue = new Queue<UnitData>();
        public readonly Queue<Lane> UnitLane = new Queue<Lane>();
        private bool _isDeploying;


        [FormerlySerializedAs("_spawnArea")]
        [Tooltip("The spawn area where the friendly unit will not spawn if another one is already present.")]
        [SerializeField, TagSelector] private string _spawnAreaTag;

        [Tooltip("The player base Tag:")]
        [SerializeField, TagSelector] private string _baseTag;

        public int MaxUnits;
        
        private readonly List<SpawnArea> _spawnAreas =  new List<SpawnArea>();
        private Transform _oneLaneSpawnPoint;
        
        private UnitData _nextUnit;
        private Lane _nextLane;
        
        private GameObject _unitReference;
        private int _currentUnitsInQueue;
        private float _timer;
        

        private void ResetQueueState()
        {
            _nextUnit = null;
            _isDeploying = false;
            _timer = 0;
            UnitQueue.Clear();
        }
        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            ResetQueueState();
            if (EnemyBasesManager.Instance.MultipleBases())
            {
                GetSpawnAreas();
            }
            else
            {
                _oneLaneSpawnPoint = FindAnyObjectByType<Lane>().PlayerUnitSpawnPosition;  
            }
        }

        private void GetSpawnAreas()
        {
            var lanes = FindObjectsByType<Lane>(FindObjectsSortMode.None);
            _spawnAreas.Clear();
            _spawnAreas.AddRange(
                FindObjectsByType<SpawnArea>(FindObjectsSortMode.None)
                    .Where(area => area.IsFriendly)
            );
        }

        private void Update()
        {
            if (!GameStates.Instance.GameIsPlaying) return;
            if (_nextUnit != null)
            {
                HandleDelayedDeployment();
            }
        }



        /// <summary>
        /// Adds the specified unit to the deployment queue.
        /// If no deployment is currently in progress, starts deploying immediately.
        /// </summary>
        public void AddUnitToDeploymentQueue(UnitData unit, Lane lane = null)
        {
            UnitQueue.Enqueue(unit);
            if (lane != null)
            {
                UnitLane.Enqueue(lane); 
            }
            
            
            OnUnitQueued?.Invoke(unit, UnitQueue.Count - 1);
            OnQueueChanged?.Invoke();
            _currentUnitsInQueue++;

            if (!_isDeploying)
            {
                ProcessNextUnitInQueue();
            }
        }

        /// <summary>
        /// Retrieves the next unit from the queue and marks deployment as in progress.
        /// If the queue is empty, resets deployment state.
        /// </summary>
        

        private void ProcessNextUnitInQueue()
        {
            if (UnitQueue.Count > 0)
            {
                _nextUnit = UnitQueue.Dequeue();

                _nextLane = UnitLane.Count > 0 ? UnitLane.Dequeue() : null;

                _isDeploying = true;

                OnQueueChanged?.Invoke();
                OnUnitReadyToDeploy?.Invoke(_nextUnit);
            }
            else
            {
                _nextUnit = null;
                _nextLane  = null;
                _isDeploying = false;
            }
        }



        /// <summary>
        /// Handles unit instantiation after a delay.
        /// Waits for sufficient time and an available spawn area before deploying.
        /// Resets timer and triggers the next deployment if available.
        /// </summary>

        private void HandleDelayedDeployment()
        {
            _timer += Time.deltaTime;
            if (!CanDeploy()) return;
            InstantiateAndInitializeUnit();
            OnUnitDeployed?.Invoke();
            _currentUnitsInQueue--;
            
            _timer = 0;
            _isDeploying = false;
            ProcessNextUnitInQueue();
        }

        private void InstantiateAndInitializeUnit()
        {
            if (_nextLane.PlayerUnitSpawnPosition == null)
            {
                _unitReference = Instantiate(_nextUnit.Prefab, _oneLaneSpawnPoint.position, _oneLaneSpawnPoint.rotation);
                if (_unitReference.TryGetComponent(out UnitBaseBehaviour behaviour))
                {
                    behaviour.Initialize(_nextUnit);
                    OnUnitDeployedOnLane?.Invoke(_nextLane, behaviour);
                }
            }
            else
            {
                _unitReference = Instantiate(_nextUnit.Prefab, _nextLane.PlayerUnitSpawnPosition.position, _nextLane.PlayerUnitSpawnPosition.rotation); 
                if (_unitReference.TryGetComponent(out UnitBaseBehaviour behaviour))
                {
                    behaviour.Initialize(_nextUnit, _nextLane.EnemyBase);
                    OnUnitDeployedOnLane?.Invoke(_nextLane, behaviour);
                }
            }
        }

        private bool CanDeploy()
        {
            if (_nextLane.PlayerSpawnArea == null)
            {
                return false;
            }

            if (_nextLane.PlayerSpawnArea.HasUnitInside)
            {
                return false;
            }

            return _timer >= _nextUnit.DeployDelayTime;
        }

        public bool MaxUnitCapacity()
        {
            if (UnitCounter.FriendlyCount + _currentUnitsInQueue >= MaxUnits)
            {
                OnMaxCapacity?.Invoke();
                return true;
            }
            else
            {
                return false;
            }
            
        }
        
        
    }
}




