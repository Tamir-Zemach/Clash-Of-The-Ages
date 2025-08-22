using System;
using System.Collections.Generic;
using Bases;
using Special_Attacks;
using units.Behavior;
using UnityEngine;
using VisualCues;

namespace Ui.Buttons.Deploy_Button
{
    [RequireComponent(typeof(HighlightGfx))]
    public class Lane : MonoBehaviour
    {

        public event Action<Lane> OnLaneDestroyed;
        private HighlightGfx _highlightGfx;
        
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
        
        [Header("Lane Lifecycle")] 
        public MeteorRainSpawnPos MeteorRainSpawnPosition;


        private EnemyBaseHealth _enemyBaseHealth;
        private List<UnitBaseBehaviour> _unitsOnLane = new();

        private void Awake()
        {
            _enemyBaseHealth = EnemyBase.GetComponent<EnemyBaseHealth>();
            _enemyBaseHealth.OnBaseDestroyed += DestroyLane;
            _highlightGfx  = GetComponent<HighlightGfx>();
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

        public void StartFlashing(float interval)
        {
            _highlightGfx?.StartFlashing(interval);
        }

        public void StopFlashing()
        {
            _highlightGfx?.StopFlashing();
        }

        public void ShrinkAndHide()
        {
            _highlightGfx?.ShrinkAndHide(
                growMultiplier: new Vector3(1f, 1.2f, 1.2f),
                shrinkMultiplier: new Vector3(1f, 0f, 0f)
            );
        }
        
    }
}