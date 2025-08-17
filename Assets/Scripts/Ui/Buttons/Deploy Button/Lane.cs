using System;
using System.Collections.Generic;
using System.Linq;
using Bases;
using units.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui.Buttons.Deploy_Button
{
    public class Lane : MonoBehaviour
    {
        public event Action<Lane> OnLaneDestroyed;
        
        [Header("Unit Spawn Positions")]
        public Transform PlayerUnitSpawnPosition;
        public Transform EnemyUnitSpawnPosition;
        
        [Header("Spawn Areas")]
        public SpawnArea PlayerSpawnArea;
        public SpawnArea EnemySpawnArea;
        
        [Header("Enemy Base")]
        public Transform EnemyBase;

        [Header("Lane Lifecycle")] 
        public bool IsDestroyed;


        private EnemyBaseHealth _enemyBaseHealth;
        private List<UnitBaseBehaviour> _unitsOnLane = new();

        private void Awake()
        {
            _enemyBaseHealth = EnemyBase.GetComponent<EnemyBaseHealth>();
            _enemyBaseHealth.OnBaseDestroyed += DestroyLane;
        }

        private void OnDestroy()
        {
            _enemyBaseHealth.OnBaseDestroyed -= DestroyLane;
        }

        private void DestroyLane()
        {
            IsDestroyed  = true;
            OnLaneDestroyed?.Invoke(this);
        }

        
    }
}