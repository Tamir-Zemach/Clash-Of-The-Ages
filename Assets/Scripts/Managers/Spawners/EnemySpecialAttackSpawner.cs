using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using UnityEngine;

public class EnemySpecialAttackSpawner : SceneAwareMonoBehaviour<EnemySpecialAttackSpawner>
{

    [Tooltip("Minimum time interval before an special attack can spawn (can be a decimal value).")]
    [SerializeField] private float _minSpawnTime;

    [Tooltip("Maximum time interval before an special attack can spawn (can be a decimal value).")]
    [SerializeField] private float _maxSpawnTime;

    private SpecialAttackData _specialAttack;
    private SpecialAttackSpawnPos _specialAttackSpawnPos;
    private SpecialAttackType _specialAttackType;
    private float _randomSpawnTimer;
    private float _timer;

    protected override void Awake()
    {
        base.Awake();

    }

    private void Update()
    {
        SpawnPrefabsWithRandomTime();
    }

    protected override void InitializeOnSceneLoad()
    {
        _randomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        _specialAttackSpawnPos = FindAnyObjectByType<SpecialAttackSpawnPos>();
        _specialAttack = GameDataRepository.Instance.EnemySpecialAttacks.GetData(SpecialAttackType.DestroyPath);
    }

    private void SpawnPrefabsWithRandomTime()
    {
        _timer += Time.deltaTime;
        if (CanDeploy())
        {
            Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.transform.position, _specialAttackSpawnPos.transform.rotation);
            _specialAttackSpawnPos.IsSpecialAttackAccruing = true;
            _timer = 0;
            _randomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        }
    }

    private bool CanDeploy()
    {
        if (_specialAttackSpawnPos == null)
        {
            print("_specialAttackSpawnPos is null");
            return false;
        }
        //TODO: deside what are the conditions for the enemy special attack and implement them
        return _timer >= _randomSpawnTimer && !_specialAttackSpawnPos.IsSpecialAttackAccruing;
    }


}
