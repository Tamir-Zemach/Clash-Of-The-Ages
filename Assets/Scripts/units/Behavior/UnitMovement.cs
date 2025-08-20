using System.Collections;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace units.Behavior
{
    public class UnitMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Transform _destination;
        private float _targetSpeed;
        private ManagedCoroutine _speedLerpCoroutine;

        public bool IsStopped => _agent.isStopped;
        public Vector3 CurrentDestination => _agent.destination;
        public float CurrentSpeed => _agent.speed;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        

        public void SetDestination(Transform destination = null)
        {
            if (destination == null)
            {
                _destination = null;
            }
            else
            {
                _destination = destination;
                _agent.destination = destination.position;
            }

        }

        public void StopMovement()
        {
            _agent.isStopped = true;
        }

        public void ResumeMovement(float targetSpeed, float duration = 0.5f)
        {
            _targetSpeed = targetSpeed;

            _speedLerpCoroutine?.Stop();

            _speedLerpCoroutine = CoroutineManager.Instance.StartManagedCoroutine(LerpAgentSpeed(duration));
        }

        private IEnumerator LerpAgentSpeed(float duration)
        {
            _agent.isStopped = false;
            float startSpeed = 0f;
            float elapsed = 0f;
            
            _agent.speed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _agent.speed = Mathf.Lerp(startSpeed, _targetSpeed, elapsed / duration);
                yield return null;
            }

            _agent.speed = _targetSpeed;
        }


    }
}