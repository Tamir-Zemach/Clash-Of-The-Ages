using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Structs;
using Managers.Loaders;
using Ui.Buttons.Deploy_Button;
using UnityEngine;

namespace Managers.Spawners
{
    public class UnitDeploymentQueue : SceneAwareMonoBehaviour<UnitDeploymentQueue>
    {

        public event Action<Deployment?> OnUnitReadyToDeploy;
        public event Action<Deployment?> OnUnitStartDeploying;
        public event Action OnQueueChanged;

        
        public IEnumerable<UnitData> UnitQueue
        {
            get { return _deploymentQueue.Select(deployment => deployment.Unit); }
        }
        

        private readonly Queue<Deployment> _deploymentQueue = new();
        private Deployment? _currentDeployment;
        private bool _isDeploymentInProgress;
        private float _timer;

        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            
            if (!_isDeploymentInProgress) return;
            
            _timer += Time.deltaTime;
            
            if (!CanDeployUnit()) return;
            DequeueAndInvokeDeployment();
            CompleteCurrentDeployment();
        }

        public int CurrentUnitsInQueue => _deploymentQueue.Count;
        
        
        public void EnqueueUnit(UnitData unit, Lane lane = null)
        {
            var deployment = new Deployment(unit, lane);
            _deploymentQueue.Enqueue(deployment);
            OnQueueChanged?.Invoke();
            if (!_isDeploymentInProgress)
            {
                StartDeployment();
            }
        }
        private void StartDeployment()
        {
            if (_deploymentQueue.Count > 0)
            {
                
                _currentDeployment = _deploymentQueue.Dequeue();
                _timer = 0;
                _isDeploymentInProgress = true;
                OnUnitStartDeploying?.Invoke(_currentDeployment);
                OnQueueChanged?.Invoke();
            }
            else
            {
                ResetDeploymentState();
            }
        }

        private void DequeueAndInvokeDeployment()
        {
            _isDeploymentInProgress = false;
            OnQueueChanged?.Invoke();
            OnUnitReadyToDeploy?.Invoke(_currentDeployment);
        }


        private void CompleteCurrentDeployment()
        {
            _currentDeployment = null;
            _isDeploymentInProgress = false;
            
            if (_deploymentQueue.Count > 0)
            {
                StartDeployment();
            }
        }
        
        private void ResetDeploymentState()
        {
            _currentDeployment = null;
            _isDeploymentInProgress = false;
        }
        
        
        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _timer = 0;
            ClearAllDeploymentQueues();
        }
        
        private void ClearAllDeploymentQueues()
        {
            _deploymentQueue.Clear();
            ResetDeploymentState();
        }

        private bool CanDeployUnit()
        {
            return _currentDeployment != null && _timer >= _currentDeployment.Value.Unit.DeployDelayTime;
        }
        
    }
}