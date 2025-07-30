using Assets.Scripts.BackEnd.BaseClasses;

using System.Collections.Generic;
using System.Linq;
using Managers.Spawners;
using turrets;
using UnityEngine;

public class EnemyTurretSlotSpawner : EnemySpawner<EnemyTurretSlotSpawner>
{
    public System.Action<TurretSpawnPoint> OnTurretSlotActivated;
    protected override float RandomSpawnTimer { get; set ; }
    protected override float Timer { get; set; } = 0;

    public List<TurretSpawnPoint> TurretSpawnPoints { get; private set; }

    private bool _isSlotWaitingToBeAdded = false;

    protected override void Awake()
    {
        base.Awake();
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
    }

    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        _isSlotWaitingToBeAdded = true;
        EnemyTurretSpawner.Instance.OnTurretPlaced += StartTimer;
        GetAllEnemyTurretSpawnPoints();
        ResetTimer();
    }

    private void StartTimer()
    {
        ResetTimer();
        if (HasSlotToUnlock())
        {
            _isSlotWaitingToBeAdded = true;
        }

    }


    private void Update()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        if (!_isSlotWaitingToBeAdded)  return; 
        CheckIfSpawnable();
    }
    private void CheckIfSpawnable()
    {
        Timer += Time.deltaTime;
        if (CanDeploy())
        {
            SpawnRandomTurretSlot();
        }

    }


    private void SpawnRandomTurretSlot()
    {

        var AvailableSlot = GetAvailableTurretSlot();
        if (AvailableSlot != null)
        {
            AvailableSlot.IsUnlocked = true;
            AvailableSlot.ShowHighlight(true);

            OnTurretSlotActivated?.Invoke(AvailableSlot);

            ResetTimer();
            _isSlotWaitingToBeAdded = false;
        }
    }
    private void GetAllEnemyTurretSpawnPoints()
    {
        TurretSpawnPoints = FindObjectsByType<TurretSpawnPoint>(FindObjectsSortMode.None)
                                .Where(spawnPoint => !spawnPoint.IsFriendly)
                                .ToList();
    }



    private TurretSpawnPoint GetAvailableTurretSlot()
    {
        return TurretSpawnPoints.FirstOrDefault(spawnPoint => !spawnPoint.IsUnlocked);
    }

    protected override bool CanDeploy()
    {
        //TODO: deside what are the conditions for the enemy turret slot Spawner and implement them
        return Timer >= RandomSpawnTimer;
    }

    private bool HasSlotToUnlock()
    {
        return TurretSpawnPoints.Any(spawnPoint => !spawnPoint.IsUnlocked);
    }

    private void ResetTimer()
    {
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        Timer = 0;
    }

}
