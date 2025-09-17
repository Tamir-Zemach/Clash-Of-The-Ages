using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;

namespace Managers
{
    public class GameManager : SceneAwareMonoBehaviour<GameManager>
    {
        public delegate void AgeUpgradeDelegate(List<LevelUpDataBase> data);
        public event AgeUpgradeDelegate OnAgeUpgrade;

        [Header("Player Starting Stats")]
        [Tooltip("Amount of money the player starts with at the beginning of the game.")]
        [Min(0)]
        [SerializeField] private int _startingMoney;
        
        [Tooltip("Amount of Experience the player needs to gain to level up in the beginning of the game.")]
        [Min(1)]
        [SerializeField] private int _startingExpToLevelUp;

        [Tooltip("Initial health points for the player.")]
        [Min(1)]
        [SerializeField] private int _startingHealth;

        [Header("Level Up Settings")]
        [Tooltip("Multiplier applied to EXP required for each level up.")]
        [SerializeField, Range(1f, 3f)] private float _expLevelUpMultiplier = 1.4f;
        
        [Header("Enemy Health Per Level")]
        [Tooltip("Enemy base health for Level 1.")]
        [Min(1)]
        [SerializeField] private int _level1EnemyHealth;


        [Tooltip("Enemy base health for Level 2.")]
        [Min(1)]
        [SerializeField] private int _level2EnemyHealth;

        [Tooltip("Enemy base health for Level 3.")]
        [Min(1)]
        [SerializeField] private int _level3EnemyHealth;


        private List<LevelUpDataGroup> _levelUpData;

        private static bool _subscribedOnce = false;

        protected override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();

            if (!_subscribedOnce)
            {
                EnemyHealth.Instance.OnEnemyDied += NextLevel;
                PlayerHealth.Instance.OnDying += GameOver;
                PlayerExp.Instance.OnLevelUp += UpgradePopUp;
                GameStates.Instance.OnGameReset += ResetGame;
                _subscribedOnce = true;
            }

            GetData();
            StartGame();
        }

        private void GameOver()
        {
            LevelLoader.Instance.LoadGameOver();
            GameStates.Instance.EndGame();
            GlobalUnitCounter.Instance.ResetCounts();
        }

        private void ResetGame()
        {
            PlayerExp.Instance.ResetExp();
            GlobalUnitCounter.Instance.ResetCounts();
        }

        protected override void InitializeOnSceneLoad()
        {
            if ((int)AgeUpgrade.Instance.CurrentPlayerAge < LevelLoader.Instance.LevelIndex)
            {
                //UpgradePlayerAge();
            }
        }


        private void GetData()
        {
            _levelUpData = GameDataRepository.Instance.PlayerLevelUpData;
        }
        private void StartGame()
        {
            PlayerCurrency.Instance.SetMoney(_startingMoney);
            PlayerHealth.Instance.SetMaxHealth(_startingHealth);
            PlayerHealth.Instance.FullHealth();
            PlayerExp.Instance.SetExp(0);
            PlayerExp.Instance.SetExpToLevelUp(_startingExpToLevelUp);
            ResetEnemyHealth();
        }

        public void NextLevel()
        {
            LevelLoader.Instance.LoadNextLevel();
            ResetEnemyHealth();
        }

        public void ResetEnemyHealth()
        {
            EnemyHealth.Instance.SetMaxHealth(SetEnemyBaseHealthForCurrentLevel(LevelLoader.Instance.LevelIndex));
            EnemyHealth.Instance.FullHealth();
        }

        public int SetEnemyBaseHealthForCurrentLevel(int levelIndex)
        {
            switch (levelIndex)
            {
                case 0: return _level1EnemyHealth;
                case 1: return _level1EnemyHealth;
                case 2: return _level2EnemyHealth;
                case 3: return _level3EnemyHealth;
                default:
                    Debug.LogWarning("Invalid level. Defaulting to Level 1 health.");
                    return _level1EnemyHealth;
            }
        }

        public void UpgradePlayerAge()
        {
            AgeUpgrade.Instance.AdvanceAge(isFriendly: true);
            PlayerHealth.Instance.FullHealth();

            var dataGroup = _levelUpData.FirstOrDefault(g => g.AgeStage == AgeUpgrade.Instance.CurrentPlayerAge);
            if (dataGroup != null)
            {
                OnAgeUpgrade?.Invoke(dataGroup.LevelUpEntries);
            }
            else
            {
                Debug.LogWarning($"No LevelUpDataGroup found for AgeStage: {AgeUpgrade.Instance.CurrentPlayerAge}");
            }
        }

        public void UpgradePopUp()
        {
            UpgradePopup.Instance.ShowPopup();
            PlayerExp.Instance.SetExp(0);
            var expToLevelUp = Mathf.Max(1, Mathf.RoundToInt(PlayerExp.Instance.ExpToLevelUp * _expLevelUpMultiplier));
            PlayerExp.Instance.SetExpToLevelUp(expToLevelUp);
        }
        
        


    }
}