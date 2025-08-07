using UnityEngine;
using BackEnd.Economy;
using Configuration;
using Managers.Spawners;

namespace Debugging
{
    /// <summary>
    /// Provides runtime debug feedback for enemy aggression triggers and spawn timer changes.
    /// Attach this to any GameObject in the scene for live console output.
    /// </summary>
    public class EnemyAggressionDebugger : MonoBehaviour
    {
        [Tooltip("Reference to the EnemyConfiguration component to monitor.")]
        [SerializeField] private EnemyConfiguration _enemyConfig;

        private void OnEnable()
        {
            EnemyHealth.OnDroppedBelowHalfHealth += LogHalfHealthAggression;
            EnemyHealth.OnEnemyHealthChanged += LogHealthChange;
            EnemyHealth.Instance.OnEnemyDied += LogReset;
        }

        private void OnDisable()
        {
            EnemyHealth.OnDroppedBelowHalfHealth -= LogHalfHealthAggression;
            EnemyHealth.OnEnemyHealthChanged -= LogHealthChange;
            EnemyHealth.Instance.OnEnemyDied -= LogReset;
        }

        private void LogHalfHealthAggression()
        {
            Debug.Log("<color=orange>[Aggression Triggered]</color> Enemy dropped below half health.");
        }

        private void LogHealthChange()
        {
            int current = EnemyHealth.Instance.CurrentHealth;
            int max = EnemyHealth.Instance.MaxHealth;
            Debug.Log($"[Health Update] Current: {current}, Max: {max}");
        }

        private void LogReset()
        {
            Debug.Log("<color=cyan>[Aggression Reset]</color> Enemy died. Spawn timers restored.");
        }

        private void Update()
        {
            if (_enemyConfig == null) return;

            // Optional: live display of current spawn timers
            Debug.Log($"[Spawn Timers] Unit: {EnemyUnitSpawner.Instance.MinSpawnTime:F2}-{EnemyUnitSpawner.Instance.MaxSpawnTime:F2}, " +
                      $"Turret: {EnemyTurretSpawner.Instance.MinSpawnTime:F2}-{EnemyTurretSpawner.Instance.MaxSpawnTime:F2}, " +
                      $"Slot: {EnemyTurretSlotSpawner.Instance.MinSpawnTime:F2}-{EnemyTurretSlotSpawner.Instance.MaxSpawnTime:F2}, " +
                      $"Special: {EnemySpecialAttackSpawner.Instance.MinSpawnTime:F2}-{EnemySpecialAttackSpawner.Instance.MaxSpawnTime:F2}");
        }
    }
}