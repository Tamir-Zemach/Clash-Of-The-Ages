using Assets.Scripts.BackEnd.BaseClasses;
using Assets.Scripts.BackEnd.Enems;
using System.Collections.Generic;
using BackEnd.Data__ScriptableOBj_;
using units.Behavior;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemyUnitSpawner : EnemySpawner<EnemyUnitSpawner>
{

    [Tooltip("Tag used to identify the enemy base in the scene.")]
    [SerializeField, TagSelector] private string _spawnAreaTag;

    [Tooltip("Maximum number of enemies allowed to spawn.")]
    [SerializeField] private int _maxEnemies;

    private SpawnArea _enemySpawnArea;
    private Transform _enemySpawnPoint;
    private List<UnitData> enemyUnits;
    protected override float RandomSpawnTimer { get; set; }
    protected override float Timer { get; set ; }

    protected override void Awake()
    {
        base.Awake();
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
    }
    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        enemyUnits = GameDataRepository.Instance.EnemyUnits;
        GameObject areaGO = GameObject.FindGameObjectWithTag(_spawnAreaTag);
        if (areaGO == null)
        {
            Debug.LogWarning($"[EnemySpawner] No GameObject with tag '{_spawnAreaTag}' found in scene.");
            return;
        }

        _enemySpawnArea = areaGO.GetComponent<SpawnArea>();
        if (_enemySpawnArea == null)
        {
            Debug.LogWarning($"[EnemySpawner] GameObject tagged '{_enemySpawnArea}' is missing SpawnArea component.");
            return;
        }

        _enemySpawnPoint = _enemySpawnArea.GetComponentInParent<Transform>();
        if (_enemySpawnPoint == null)
        {
            Debug.LogWarning("[EnemySpawner] Failed to locate parent transform for spawn point.");
        }
    }

    private void Update()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        SpawnPrefabsWithRandomTime();

    }

    private void SpawnPrefabsWithRandomTime()
    {
        Timer += Time.deltaTime;
        if (CanDeploy())
        {
            SpawnRandomEnemyPrefab();
            Timer = 0;
            RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        }
    }

    private void SpawnRandomEnemyPrefab()
    {

        if (enemyUnits == null || enemyUnits.Count == 0) return;

        UnitData randomEnemyData = enemyUnits[Random.Range(0, enemyUnits.Count)];

        GameObject enemyReference = Instantiate(
            randomEnemyData.Prefab,
            _enemySpawnPoint.position,
            _enemySpawnPoint.rotation
        );

        UnitBaseBehaviour behaviour = enemyReference.GetComponent<UnitBaseBehaviour>();

        if (behaviour != null)
        {
            behaviour.Initialize(randomEnemyData);
        }
        else
        {
            Debug.LogWarning("UnitBaseBehaviour not found on spawned enemy prefab.");
        }
    }

    protected override bool CanDeploy()
    {
        return Timer >= RandomSpawnTimer && _enemySpawnArea != null && !_enemySpawnArea._hasUnitInside && UnitCounter.EnemyCount < _maxEnemies;
    }


}