using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using Managers.Loaders;
using UnityEngine;

namespace Managers
{
    public class EnemyAgeManager : SingletonMonoBehaviour<EnemyAgeManager>
    {
        public delegate void AgeUpgradeDelegate(List<LevelUpDataBase> data);
        public event AgeUpgradeDelegate OnAgeUpgrade;
        private List<LevelUpDataGroup> _levelUpData;

        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            if (GameDataRepository.Instance.EnemyLevelUpData != null)
            {
                _levelUpData = GameDataRepository.Instance.EnemyLevelUpData;
            }
        }


        public void UpgradeEnemyAge()
        {
            AgeUpgrade.Instance.AdvanceAge(isFriendly: false);
            EnemyHealth.Instance.FullHealth();

            var dataGroup = _levelUpData.FirstOrDefault(g => g.AgeStage == AgeUpgrade.Instance.CurrentEnemyAge);
            if (dataGroup != null)
            {
                OnAgeUpgrade?.Invoke(dataGroup.LevelUpEntries);
            }
            else
            {
                Debug.LogWarning($"No LevelUpDataGroup found for AgeStage: {AgeUpgrade.Instance.CurrentEnemyAge}");
            }
        }


    }
}
