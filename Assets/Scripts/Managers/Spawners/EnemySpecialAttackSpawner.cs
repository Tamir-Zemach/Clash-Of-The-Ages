using Assets.Scripts.BackEnd.BaseClasses;
using Assets.Scripts.Data;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Data__ScriptableOBj_;
using Special_Attacks;
using UnityEngine;

public class EnemySpecialAttackSpawner : EnemySpawner<EnemySpecialAttackSpawner>
{
    private SpecialAttackData _specialAttack;
    private SpecialAttackSpawnPos _specialAttackSpawnPos;
    private SpecialAttackType _specialAttackType;

    protected override float RandomSpawnTimer { get; set; }

    protected override float Timer { get; set; }
    

    private void Update()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        SpawnPrefabsWithRandomTime();
    }

    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        _specialAttackSpawnPos = FindAnyObjectByType<SpecialAttackSpawnPos>();
        _specialAttack = GameDataRepository.Instance.EnemySpecialAttacks.GetData(SpecialAttackType.DestroyPath);
    }
    protected override bool CanDeploy()
    {
        if (_specialAttackSpawnPos == null)
        {
            return false;
        }
        //TODO: decide what are the conditions for the enemy special attack and implement them
        
        return Timer >= RandomSpawnTimer && !_specialAttackSpawnPos.IsSpecialAttackAccruing;
    }

    private void SpawnPrefabsWithRandomTime()
    {
        Timer += Time.deltaTime;
        if (!CanDeploy()) return;
        SpawnSpecialAttack();
        _specialAttackSpawnPos.IsSpecialAttackAccruing = true;
        Timer = 0;
        RandomSpawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
    }


    private void SpawnSpecialAttack()
    {
        var specialAttack = Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.transform.position, _specialAttackSpawnPos.transform.rotation);
        var behaviour = specialAttack.GetComponent<SpecialAttackBaseBehavior>();

        if (behaviour != null)
        {
            behaviour.Initialize(_specialAttack, _specialAttackSpawnPos);
        }
        else
        {
            Debug.LogWarning("SpecialAttackBaseBehaviour not found on spawned enemy prefab.");
        }

    }

}
