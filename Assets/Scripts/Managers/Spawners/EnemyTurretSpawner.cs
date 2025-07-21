using Assets.Scripts.BackEnd.BaseClasses;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Data__ScriptableOBj_;
using turrets;
using UnityEngine;

public class EnemyTurretSpawner : EnemySpawner<EnemyTurretSpawner>
{
    public event System.Action OnTurretPlaced;

    protected override float RandomSpawnTimer { get; set; }
    protected override float Timer { get; set; }

    private TurretSpawnPoint _availableSpawnPoint;

    private List<TurretData> _enemyTurrets;

    private bool _isTurretWaitingToSpawn = false;    

    protected override void Awake()
    {
        base.Awake();
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
    }

    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        _enemyTurrets = GameDataRepository.Instance.EnemyTurrets;
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        EnemyTurretSlotSpawner.Instance.OnTurretSlotActivated += StartTimer;
        ResetTimer();
    }



    private void StartTimer(TurretSpawnPoint slot)
    {
        _isTurretWaitingToSpawn = true;
        _availableSpawnPoint = slot;
        ResetTimer();
    }

    private void Update()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        if (!_isTurretWaitingToSpawn) return;
        CheckIfSpawnable();
    }

    private void CheckIfSpawnable()
    {
        Timer += Time.deltaTime;
        if (CanDeploy())
        {
            SpawnRandomEnemyTurret();
        }
    }

    private void SpawnRandomEnemyTurret()
    {
        if (_enemyTurrets == null || _enemyTurrets.Count == 0) return;

        TurretData randomTurretData = _enemyTurrets[Random.Range(0, _enemyTurrets.Count)];

        GameObject enemyReference = Instantiate(
            randomTurretData.Prefab,
            _availableSpawnPoint.transform.position,
            _availableSpawnPoint.transform.rotation
        );
        enemyReference.transform.SetParent(_availableSpawnPoint.transform);
        TurretBaseBehavior behaviour = enemyReference.GetComponent<TurretBaseBehavior>();

        if (behaviour != null)
        {
            behaviour.Initialize(randomTurretData);
        }
        else
        {
            Debug.LogWarning("UnitBaseBehaviour not found on spawned enemy prefab.");
        }
        _availableSpawnPoint.HasTurret = true;
        _availableSpawnPoint = null;
        _isTurretWaitingToSpawn = false;
        OnTurretPlaced?.Invoke();
    }


    protected override bool CanDeploy()
    {
        return Timer >= RandomSpawnTimer 
            && EnemyTurretSlotSpawner.Instance.TurretSpawnPoints.Any(spawnPoint => !spawnPoint.HasTurret);
    }

    private void ResetTimer()
    {
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        Timer = 0;
    }



}
