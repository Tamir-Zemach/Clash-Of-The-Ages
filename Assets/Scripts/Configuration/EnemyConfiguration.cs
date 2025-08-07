using System.Collections;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using UnityEngine;
using BackEnd.Economy;
using BackEnd.Utilities;
using Managers.Spawners;
using UnityEngine.Serialization;

namespace Configuration
{
    /// <summary>
    /// Controls enemy aggression behavior based on health thresholds and damage spikes.
    /// Adjusts spawn rates of enemy units, turrets, and special attacks dynamically.
    /// </summary>
    public class EnemyConfiguration : MonoBehaviour
    {
        [Header("Aggression Settings")]

        [Tooltip("Multiplies the current spawn timers (MinSpawnTime and MaxSpawnTime) by this value. Lower values result in faster spawns.")]
        [Range(0f, 1f)]
        [SerializeField] private float _aggressionMultiplier = 0.5f;

        [Tooltip("If the enemy loses more than this percentage of its max health within _damageWindow seconds, aggression is triggered.")]
        [Range(0f, 1f)]
        [SerializeField] private float _damageSpikeThreshold = 0.3f;

        [Tooltip("After any damage is taken, a timer starts. If the accumulated damage during this window exceeds _damageSpikeThreshold, aggression is triggered.")]
        [SerializeField] private float _damageWindow = 2f;

        private int _lastHealth;
        private float _damageAccumulated;
        private ManagedCoroutine _damageMonitorRoutine;


        private readonly Dictionary<MonoBehaviour, (float min, float max)> _originalSpawnTimes = new();
        
        private void OnEnable()
        {
            EnemyHealth.OnDroppedBelowHalfHealth += TriggerAggression;
            EnemyHealth.Instance.OnEnemyDied += ResetAggression;
            EnemyHealth.OnEnemyHealthChanged += MonitorDamageSpike;

            _lastHealth = EnemyHealth.Instance.CurrentHealth;
        }
        
        private void OnDisable()
        {
            EnemyHealth.OnDroppedBelowHalfHealth -= TriggerAggression;
            EnemyHealth.Instance.OnEnemyDied -= ResetAggression;
            EnemyHealth.OnEnemyHealthChanged -= MonitorDamageSpike;
        }

        /// <summary>
        /// Tracks damage taken and starts a coroutine to evaluate if a damage spike occurred.
        /// </summary>
        private void MonitorDamageSpike()
        {
            int currentHealth = EnemyHealth.Instance.CurrentHealth;
            int damageTaken = _lastHealth - currentHealth;

            if (damageTaken > 0)
            {
                _damageAccumulated += damageTaken;

                if (_damageMonitorRoutine == null)
                {
                    _damageMonitorRoutine = CoroutineManager.Instance.StartManagedCoroutine(DamageWindowRoutine());
                }
            }

            _lastHealth = currentHealth;
        }

        /// <summary>
        /// Waits for the damage window duration and checks if accumulated damage exceeds the threshold.
        /// If so, triggers aggression.
        /// </summary>
        private IEnumerator DamageWindowRoutine()
        {
            yield return new WaitForSeconds(_damageWindow);

            float damageRatio = _damageAccumulated / EnemyHealth.Instance.MaxHealth;
            if (damageRatio >= _damageSpikeThreshold)
            {
                TriggerAggression();
            }

            _damageAccumulated = 0;
            _damageMonitorRoutine = null;
        }
        
        
        private void TriggerAggression()
        {
            ApplyAggressionToSpawner(EnemyUnitSpawner.Instance);
            ApplyAggressionToSpawner(EnemyTurretSpawner.Instance);
            ApplyAggressionToSpawner(EnemyTurretSlotSpawner.Instance);
            ApplyAggressionToSpawner(EnemySpecialAttackSpawner.Instance);
        }

        /// <summary>
        /// Reduces the spawn timers of a given spawner using the aggression multiplier.
        /// Stores original values for later reset.
        /// </summary>
        /// <param name="spawner">The spawner instance to modify</param>
        private void ApplyAggressionToSpawner<T>(EnemySpawner<T> spawner) where T : EnemySpawner<T>
        {
            if (spawner == null) return;

            if (!_originalSpawnTimes.ContainsKey(spawner))
            {
                _originalSpawnTimes[spawner] = (spawner.MinSpawnTime, spawner.MaxSpawnTime);
            }

            spawner.MinSpawnTime *= _aggressionMultiplier;
            spawner.MaxSpawnTime *= _aggressionMultiplier;
        }
        
        private void ResetAggression()
        {
            foreach (var kvp in _originalSpawnTimes)
            {
                if (kvp.Key is EnemySpawner<MonoBehaviour> spawner)
                {
                    spawner.MinSpawnTime = kvp.Value.min;
                    spawner.MaxSpawnTime = kvp.Value.max;
                }
            }
            // Stop any running damage monitor coroutine
            _damageMonitorRoutine?.Stop();
            _damageMonitorRoutine = null;

            _originalSpawnTimes.Clear();
        }
    }
}