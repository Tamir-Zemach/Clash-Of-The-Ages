using UnityEngine;
using BackEnd.Economy;
using Managers.Spawners;
using TMPro;

namespace Debugging
{
    public class EnemyAggressionDebugUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _halfHealthText;
        [SerializeField] private TextMeshProUGUI _resetHealthText;
        [SerializeField] private TextMeshProUGUI _spawnersText;

        private void OnEnable()
        {
            EnemyHealth.OnDroppedBelowHalfHealth += LogHalfHealthAggression;
            EnemyHealth.Instance.OnEnemyDied += LogReset;
        }

        private void OnDisable()
        {
            EnemyHealth.OnDroppedBelowHalfHealth -= LogHalfHealthAggression;
            EnemyHealth.Instance.OnEnemyDied -= LogReset;
        }

        private void LogHalfHealthAggression()
        {
            _halfHealthText.text = "<color=orange>[Aggression Triggered]</color> Enemy dropped below half health.";
        }
        

        private void LogReset()
        {
            _resetHealthText.text = "<color=cyan>[Aggression Reset]</color> Enemy died. Spawn timers restored.";
        }

        private void LogSpawners()
        {
            if (EnemyUnitSpawner.Instance == null ||
                EnemyTurretSpawner.Instance == null || 
                EnemyTurretSlotSpawner.Instance == null ||
                EnemySpecialAttackSpawner.Instance == null) return;
            _spawnersText.text = $"[Spawn Timers] Unit: {EnemyUnitSpawner.Instance.MinSpawnTime:F2}-{EnemyUnitSpawner.Instance.MaxSpawnTime:F2}, " +
                      $"Turret: {EnemyTurretSpawner.Instance.MinSpawnTime:F2}-{EnemyTurretSpawner.Instance.MaxSpawnTime:F2}, " +
                      $"Slot: {EnemyTurretSlotSpawner.Instance.MinSpawnTime:F2}-{EnemyTurretSlotSpawner.Instance.MaxSpawnTime:F2}, " +
                      $"Special: {EnemySpecialAttackSpawner.Instance.MinSpawnTime:F2}-{EnemySpecialAttackSpawner.Instance.MaxSpawnTime:F2}";
        }
        private void Update()
        {
            LogSpawners();
        }
    }
}