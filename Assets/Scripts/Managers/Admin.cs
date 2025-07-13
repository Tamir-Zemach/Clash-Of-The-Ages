using Assets.Scripts;
using Assets.Scripts.Enems;
using Assets.Scripts.Managers;
using UnityEngine;

public class Admin : PersistentMonoBehaviour<Admin>
{

    public int _moneyToAdd;
    public int _moneyToSubstract;
    public int _healthToAdd;

    public int _gameSpeed = 1 ;

    public bool _displayFrienlyUnitParametersInConsole;
    public bool _displayEnemyUnitParametersInConsole;
    public bool _displayHealthInConsole;
    public bool _displayUnitCounterInConsole;
    public bool _easyMode;

    [SerializeField] private float _easyModeMinSpawnTime;
    [SerializeField] private float _easyModeMaxSpawnTime;


    private void AdminFunctions()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerCurrency.Instance.AddMoney(_moneyToAdd);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerCurrency.Instance.SubtractMoney(_moneyToSubstract);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerHealth.Instance.AddHealth(_healthToAdd);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameSpeedControl();
        }
        if (_easyMode)
        {
            EasyMode();
        }
        if (_displayFrienlyUnitParametersInConsole)
        {
            DisplayFriendlyUnitParameters();
        }
        if (_displayEnemyUnitParametersInConsole)
        {
            DisplayEnemyUnitParameters();
        }
        if (_displayUnitCounterInConsole)
        {
            DisplayUnitCounterInConsole();
        }
        if (_displayHealthInConsole)
        {
            PlayerHealth.Instance.DisplyHealthInConsole();
        }
    }

    private void GameSpeedControl() 
    {
        Time.timeScale = _gameSpeed;
    }
    private void EasyMode()
    {
        if (EnemySpawner.Instance != null) 
        {
            EnemySpawner.Instance.EasyMode(_easyModeMinSpawnTime, _easyModeMaxSpawnTime);
        }

    }

    public void DisplayFriendlyUnitParameters()
    {
        foreach (var unit in GameDataRepository.Instance.FriendlyUnits)
        {
            Debug.Log($"{unit.name} " +
                      $"Unit Cost: {unit.Cost}, " +
                      $"Health: {unit.Health}, " +
                      $"Speed: {unit.Speed}, " +
                      $"Strength: {unit.Strength}, " +
                      $"Attack Speed (Initial Attack Delay): {unit.InitialAttackDelay}, " +
                      $"Range: {unit.Range}");
        }
    }
    public void DisplayEnemyUnitParameters()
    {
        foreach (var unit in GameDataRepository.Instance.EnemyUnits)
        {
            Debug.Log($"{unit.name} " +
                      $"Reward: {unit._moneyWhenKilled}, " +
                      $"Health: {unit.Health}, " +
                      $"Speed: {unit.Speed}, " +
                      $"Strength: {unit.Strength}, " +
                      $"Attack Speed (Initial Attack Delay): {unit.InitialAttackDelay}, " +
                      $"Range: {unit.Range}");
        }
    }

    public void DisplayUnitCounterInConsole()
    {
        print($"Friendly Units: {UnitCounter.FriendlyCount}," +
              $"Enemy Units: {UnitCounter.EnemyCount}");
    }


    private void Update()
    {
        AdminFunctions();
    }

    public void UpgradeAgeToLevel()
    {
        if ((int)AgeUpgrade.Instance.CurrentPlayerAge < LevelLoader.Instance.LevelIndex)
        {
            GameManager.Instance.UpgradePlayerAge();
        }
    }

}
