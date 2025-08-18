using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Enums;
using Special_Attacks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Spawners
{
    public class EnemySpecialAttackSpawner : EnemySpawner<EnemySpecialAttackSpawner>
    {
        private SpecialAttackData _specialAttack;
        
        private List<MeteorRainSpawnPos> _meteorRainSpawnPosList = new ();
        private int _randomSpawnPosIndex;
        
        private SpecialAttackType _specialAttackType;
        
        private bool _meteorRainAccruing;

        protected override float RandomSpawnTimer { get; set; }

        protected override float Timer { get; set; }
    

        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu() || !GameStates.Instance.GameIsPlaying) return;
            SpawnPrefabsWithRandomTime();
        }

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _meteorRainSpawnPosList = FindObjectsByType<MeteorRainSpawnPos>(FindObjectsSortMode.None).ToList();
            _specialAttack = GameDataRepository.Instance.EnemySpecialAttacks.GetData(SpecialAttackType.DestroyPath);
            ResetRandoms();
            MeteorRainSpawnPos.OnMeteorRainAccruing += MeteorRainInProgress;
            MeteorRainSpawnPos.OnMeteorRainEnding += MeteorRainInEnded;
        }

        private void OnDestroy()
        {
            MeteorRainSpawnPos.OnMeteorRainAccruing -= MeteorRainInProgress;
            MeteorRainSpawnPos.OnMeteorRainEnding -= MeteorRainInEnded;
        }

        private void MeteorRainInEnded()
        {
            _meteorRainAccruing = false;
        }

        private void MeteorRainInProgress()
        {
            _meteorRainAccruing = true;
        }

        protected override bool CanDeploy()
        {
            if (_meteorRainSpawnPosList == null)
            {
                return false;
            }
        
            return Timer >= RandomSpawnTimer && !_meteorRainAccruing;
        }

        private void SpawnPrefabsWithRandomTime()
        {
            Timer += Time.deltaTime;
            if (!CanDeploy() || _meteorRainSpawnPosList.Count == 0) return;

            SpawnSpecialAttack();
            _meteorRainSpawnPosList[_randomSpawnPosIndex].ApplySpecialAttack();
            ResetRandoms();
        }

        private void ResetRandoms()
        {
            Timer = 0;
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
            _randomSpawnPosIndex = Random.Range(0, _meteorRainSpawnPosList.Count);
        }


        private void SpawnSpecialAttack()
        {

            if (_randomSpawnPosIndex < 0 || _meteorRainSpawnPosList[_randomSpawnPosIndex] == null || _specialAttack == null) return;
            var specialAttack = Instantiate(_specialAttack.Prefab, _meteorRainSpawnPosList[_randomSpawnPosIndex].transform.position, _meteorRainSpawnPosList[_randomSpawnPosIndex].transform.localRotation);
            var behaviour = specialAttack.GetComponent<SpecialAttackBaseBehavior>();

            if (behaviour != null)
            {
                behaviour.Initialize(_specialAttack, _meteorRainSpawnPosList[_randomSpawnPosIndex]);
            }
            else
            {
                Debug.LogWarning("SpecialAttackBaseBehaviour not found on spawned enemy prefab.");
            }

        }

    }
}
