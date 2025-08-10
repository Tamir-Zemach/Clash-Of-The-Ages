using System;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Project_inspector_Addons;
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
        public static event Action OnMaxCapacity;

        public readonly Queue<UnitData> UnitQueue = new Queue<UnitData>();
        private bool _isDeploying;


        [FormerlySerializedAs("_spawnArea")]
        [Tooltip("The spawn area where the friendly unit will not spawn if another one is already present.")]
        [SerializeField, TagSelector] private string _spawnAreaTag;

        [Tooltip("The player base Tag:")]
        [SerializeField, TagSelector] private string _baseTag;

        public int MaxUnits;
        
        private SpawnArea _spawnArea;
        private Transform _unitSpawnPoint;
        private UnitData _nextCharacter;
        private GameObject _unitReference;
        private int _currentUnitsInQueue;
        private float _timer;
        

        private void ResetQueueState()
        {
            _nextCharacter = null;
            _isDeploying = false;
            _timer = 0;
            UnitQueue.Clear();
        }
        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            ResetQueueState();
            
            var areaGo = GameObject.FindGameObjectWithTag(_spawnAreaTag);
            if (areaGo == null)
            {
                Debug.LogWarning($"[DeployManager] No GameObject with tag '{_spawnAreaTag}' found in scene.");
                return;
            }

            _spawnArea = areaGo.GetComponent<SpawnArea>();
            if (_spawnArea == null)
            {
                Debug.LogWarning($"[DeployManager] GameObject tagged '{_spawnAreaTag}' is missing SpawnArea component.");
                return;
            }

            _unitSpawnPoint = _spawnArea.GetComponentInParent<Transform>();
            if (_unitSpawnPoint == null)
            {
                Debug.LogWarning("[DeployManager] Failed to locate parent transform for spawn point.");
            }

        }

        private void Update()
        {
            if (!GameStates.Instance.GameIsPlaying) return;
            if (_nextCharacter != null)
                HandleDelayedDeployment();
        }



        /// <summary>
        /// Adds the specified unit to the deployment queue.
        /// If no deployment is currently in progress, starts deploying immediately.
        /// </summary>
        public void AddUnitToDeploymentQueue(UnitData unit)
        {

            UnitQueue.Enqueue(unit);
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
                _nextCharacter = UnitQueue.Dequeue();
                _isDeploying = true;

                OnQueueChanged?.Invoke();
                OnUnitReadyToDeploy?.Invoke(_nextCharacter);
            }
            else
            {
                _nextCharacter = null;
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
            _unitReference = Instantiate(_nextCharacter.Prefab, _unitSpawnPoint.position, _unitSpawnPoint.rotation);

            if (_unitReference.TryGetComponent(out UnitBaseBehaviour behaviour))
            {
                behaviour.Initialize(_nextCharacter);
            }

            OnUnitDeployed?.Invoke();
            _currentUnitsInQueue--;
            
            _timer = 0;
            _isDeploying = false;
            ProcessNextUnitInQueue();
        }

        private bool CanDeploy()
        {
            return _timer >= _nextCharacter.DeployDelayTime && !_spawnArea._hasUnitInside;
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




