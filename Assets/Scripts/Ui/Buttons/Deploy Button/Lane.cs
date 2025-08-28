using System;
using System.Collections;
using System.Collections.Generic;
using Bases;
using Special_Attacks;
using units.Behavior;
using UnityEngine;
using VisualCues;

namespace Ui.Buttons.Deploy_Button
{
    [RequireComponent(typeof(TerrainTextureSwapper))]
    public class Lane : MonoBehaviour
    {
        public event Action<Lane> OnLaneDestroyed;

        private TerrainTextureSwapper _textureSwapper;

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

        private void Awake()
        {
            _enemyBaseHealth = EnemyBase.GetComponent<EnemyBaseHealth>();
            _enemyBaseHealth.OnBaseDestroyed += DestroyLane;

            _textureSwapper = GetComponent<TerrainTextureSwapper>();
        }

        private void OnDestroy()
        {
            _enemyBaseHealth.OnBaseDestroyed -= DestroyLane;
        }

        private void DestroyLane()
        {
            IsDestroyed = true;
            OnLaneDestroyed?.Invoke(this);
        }

        public void StartFlashing(float interval)
        {
            _textureSwapper?.StartFlashing(interval);
        }

        public void StopFlashing()
        {
            _textureSwapper?.StopFlashing();
        }

        public void FlashOnce()
        {
            _textureSwapper?.FlashOnce();
        }
        
    }
}