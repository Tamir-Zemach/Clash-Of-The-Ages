using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using Assets.Scripts.units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : SceneAwareMonoBehaviour<GameManager>
{
    public delegate void AgeUpgradeDelegate(List<LevelUpDataBase> data);
    public event AgeUpgradeDelegate OnAgeUpgrade;

    [Header("Player Starting Stats")]
    [Tooltip("Amount of money the player starts with at the beginning of the game.")]
    [Min(0)]
    [SerializeField] private int _startingMoney;

    [Tooltip("Initial health points for the player.")]
    [Min(1)]
    [SerializeField] private int _startingHealth;

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

    protected override void Awake()
    {
        base.Awake();
        EnemyHealth.Instance.OnEnemyDied += NextLevel;
        GetData();
        StartGame();
    }
    protected override void InitializeOnSceneLoad()
    {
        if ((int)AgeUpgrade.Instance.CurrentPlayerAge < LevelLoader.Instance.LevelIndex)
        {
            UpgradePlayerAge();
        }
    }



    private void Update()
    {
        if (PlayerHealth.Instance.PlayerDied())
        {
            GameOver();
        }
    }
    private void GetData()
    {
        _levelUpData = GameDataRepository.Instance.PlayerLevelUpData;
    }
    private void StartGame()
    {
        PlayerCurrency.Instance.AddMoney(_startingMoney);
        PlayerHealth.Instance.SetMaxHealth(_startingHealth);
        PlayerHealth.Instance.FullHealth();
    }

    public void NextLevel()
    {
        if (LevelLoader.Instance.LevelIndex != 0)
        {
            UpgradePlayerAge();
        }
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

    private void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
    }


}