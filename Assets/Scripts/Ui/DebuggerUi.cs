using System;
using BackEnd.Base_Classes;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using Managers;
using Managers.Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui
{
    public class DebuggerUi : SingletonMonoBehaviour<DebuggerUi>
    {
        public GameObject DebuggerPanel;

        [Header("Unit Info Displays")]
        public TMP_Text FriendlyUnitText;
        public TMP_Text EnemyUnitText;
        public TMP_Text UnitCounterText;
        
        [Header("Enemy Aggression Info")]
        public TextMeshProUGUI HalfHealthText;
        public TextMeshProUGUI ResetHealthText;
        public TextMeshProUGUI SpawnersText;
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
        

        protected override void Awake()
        {
            base.Awake();
            DebuggerPanel.SetActive(false);
        }
        private void Update()
        {
            LogSpawners();
            DisplayParams();
        }
        

        public void ShowPanel(GameObject panel)
        {
            panel.SetActive(!panel.activeSelf);
        }
        
        private void DisplayParams()
        {
            DisplayEnemyUnitParameters();
            DisplayFriendlyUnitParameters();
            DisplayUnitCounter();
        }

        private void DisplayFriendlyUnitParameters()
        {
            var info = "";
            foreach (var unit in GameDataRepository.Instance.FriendlyUnits)
            {
                info += $"{unit.name}\n" +
                        $"Health: {unit.Health}, Speed: {unit.Speed}, Strength: {unit.MinStrength} - {unit.MaxStrength}\n" +
                        $"Attack Speed: {unit.InitialAttackDelay}, Range: {unit.Range}\n\n";
            }
            FriendlyUnitText.text = info;
        }

        private void DisplayEnemyUnitParameters()
        {
            var info = "";
            foreach (var unit in GameDataRepository.Instance.EnemyUnits)
            {
                info += $"{unit.name}\n" +
                        $"Reward: {unit.MoneyWhenKilled}, Health: {unit.Health}, Speed: {unit.Speed}, Strength: {unit.MinStrength} - {unit.MaxStrength}\n" +
                        $"Attack Speed: {unit.InitialAttackDelay}, Range: {unit.Range}\n\n";
            }
            EnemyUnitText.text = info;
        }

        private void DisplayUnitCounter()
        {
            UnitCounterText.text = $"Friendly Units: {GlobalUnitCounter.Instance.FriendlyCount}\n" +
                                   $"Enemy Units: {GlobalUnitCounter.Instance.EnemyCount}";
        }
        
        private void LogHalfHealthAggression()
        {
            HalfHealthText.text = "<color=orange>[Aggression Triggered]</color> Enemy dropped below half health.";
        }
        

        private void LogReset()
        {
            ResetHealthText.text = "<color=cyan>[Aggression Reset]</color> Enemy died. Spawn timers restored.";
        }

        private void LogSpawners()
        {
            SpawnersText.text = $"[Spawn Timers] Unit: {EnemyUnitSpawner.Instance.MinSpawnTime:F2}-{EnemyUnitSpawner.Instance.MaxSpawnTime:F2}, " +
                                $"Turret: {EnemyTurretSpawner.Instance.MinSpawnTime:F2}-{EnemyTurretSpawner.Instance.MaxSpawnTime:F2}, " +
                                $"Slot: {EnemyTurretSlotSpawner.Instance.MinSpawnTime:F2}-{EnemyTurretSlotSpawner.Instance.MaxSpawnTime:F2}, " +
                                $"Special: {EnemySpecialAttackSpawner.Instance.MinSpawnTime:F2}-{EnemySpecialAttackSpawner.Instance.MaxSpawnTime:F2}";
        }

        
    }
}