using System;
using BackEnd.Base_Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui
{
    public class DebuggerUi : OneInstanceMonoBehaviour<DebuggerUi>
    {
        public GameObject DebuggerPanel;

        [Header("Unit Info Displays")]
        public TMP_Text FriendlyUnitText;
        public TMP_Text EnemyUnitText;
        public TMP_Text UnitCounterText;
        protected override void Awake()
        {
            base.Awake();
            DebuggerPanel.SetActive(false);
        }

        private void Update()
        {
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
        
        public void DisplayFriendlyUnitParameters()
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

        public void DisplayEnemyUnitParameters()
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

        public void DisplayUnitCounter()
        {
            UnitCounterText.text = $"Friendly Units: {UnitCounter.FriendlyCount}\n" +
                                   $"Enemy Units: {UnitCounter.EnemyCount}";
        }

        
    }
}