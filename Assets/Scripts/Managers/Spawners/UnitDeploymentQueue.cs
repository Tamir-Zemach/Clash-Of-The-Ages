using System;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using Ui.Buttons.Deploy_Button;
using UnityEngine;

namespace Managers.Spawners
{
    public class UnitDeploymentQueue : Singleton<UnitDeploymentQueue>
    {
        #region Events

        public event Action<UnitData, int> OnUnitQueued;
        public event Action<UnitData> OnUnitReadyToDeploy;
        public event Action OnQueueChanged;

        #endregion

        #region Struct
        /// <summary>
        /// Represents a queued deployment request containing a unit and an optional lane.
        /// </summary>
        private struct DeploymentRequest
        {
            public UnitData Unit;
            public Lane Lane;

            public DeploymentRequest(UnitData unit, Lane lane)
            {
                Unit = unit;
                Lane = lane;
            }
        }
        public IEnumerable<UnitData> UnitQueue
        {
            get
            {
                foreach (var request in _deploymentQueue)
                {
                    yield return request.Unit;
                }
            }
        }

        #endregion

        #region Fields

        private readonly Queue<DeploymentRequest> _deploymentQueue = new();
        private DeploymentRequest? _currentRequest;
        private bool _isDeploymentInProgress;

        public int CurrentUnitsInQueue => _deploymentQueue.Count;
        

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a unit (and optionally, a lane) to the deployment queue.
        /// Triggers events and starts deployment if idle.
        /// </summary>
        public void EnqueueUnit(UnitData unit, Lane lane = null)
        {
            var request = new DeploymentRequest(unit, lane);
            _deploymentQueue.Enqueue(request);
            InvokeUnitQueued(unit);
            TryStartDeployment();
        }

        /// <summary>
        /// Marks the current deployment as complete and attempts to start the next one.
        /// </summary>
        public void CompleteCurrentDeployment()
        {
            ResetDeploymentState();
            TryStartNextDeployment();
        }
        
        public void ClearAllDeploymentQueues()
        {
            _deploymentQueue.Clear();
            ResetDeploymentState();
        }

        /// <summary>
        /// Returns the currently prepared unit and lane for deployment.
        /// </summary>
        public (UnitData unit, Lane lane)? GetNextUnit()
        {
            if (_currentRequest == null) return null;
            return (_currentRequest.Value.Unit, _currentRequest.Value.Lane);
        }

        #endregion

        #region Private Methods
        
        private void InvokeUnitQueued(UnitData unit)
        {
            OnUnitQueued?.Invoke(unit, _deploymentQueue.Count - 1);
            OnQueueChanged?.Invoke();
        }
        
        private void TryStartDeployment()
        {
            if (!_isDeploymentInProgress)
            {
                TryStartNextDeployment();
            }
        }
        
        private void TryStartNextDeployment()
        {
            if (_deploymentQueue.Count > 0)
            {
                PrepareNextDeployment();
            }
            else
            {
                ResetDeploymentState();
            }
        }
        
        
        private void PrepareNextDeployment()
        {
            
            _currentRequest = _deploymentQueue.Dequeue(); 
            _isDeploymentInProgress = true; 

            OnQueueChanged?.Invoke();
            OnUnitReadyToDeploy?.Invoke(_currentRequest.Value.Unit);
            
        }
        
        private void ResetDeploymentState()
        {
            _currentRequest = null;
            _isDeploymentInProgress = false;
        }

        #endregion
    }
}