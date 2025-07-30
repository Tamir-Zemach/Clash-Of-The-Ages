using Assets.Scripts;
using BackEnd.Base_Classes;
using BackEnd.Economy;
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

    public void DisplayFriendlyUnitParameters()
    {
        foreach (var unit in GameDataRepository.Instance.FriendlyUnits)
        {
            Debug.Log($"{unit.name} " +
                      $"Health: {unit.Health}, " +
                      $"Speed: {unit.Speed}, " +
                      $"Strength: {unit.MinStrength} - {unit.MaxStrength}, " +
                      $"Attack Speed (Initial Attack Delay): {unit.InitialAttackDelay}, " +
                      $"Range: {unit.Range}");
        }
    }
    public void DisplayEnemyUnitParameters()
    {
        foreach (var unit in GameDataRepository.Instance.EnemyUnits)
        {
            Debug.Log($"{unit.name} " +
                      $"Reward: {unit.MoneyWhenKilled}, " +
                      $"Health: {unit.Health}, " +
                      $"Speed: {unit.Speed}, " +
                      $"Strength: {unit.MinStrength} - {unit.MaxStrength}, " +
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

}
