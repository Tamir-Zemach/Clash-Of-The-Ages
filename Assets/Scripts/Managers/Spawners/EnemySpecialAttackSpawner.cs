using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Enums;
using Managers.Loaders;
using Special_Attacks;
using Ui.Buttons.Deploy_Button;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Spawners
{
    public class EnemySpecialAttackSpawner : EnemySpawner<EnemySpecialAttackSpawner>
    {
        private SpecialAttackData _specialAttack;
        
        private List<Lane> _lanes = new ();
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
            _lanes = LaneManager.Instance.Lanes;
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
            if (_lanes == null)
            {
                return false;
            }
        
            return Timer >= RandomSpawnTimer && !_meteorRainAccruing;
        }

        private void SpawnPrefabsWithRandomTime()
        {
            Timer += Time.deltaTime;
            if (!CanDeploy() || _lanes.Count == 0) return;

            SpawnSpecialAttack();
            _lanes[_randomSpawnPosIndex].MeteorRainSpawnPosition.ApplySpecialAttack();
            ResetRandoms();
        }

        private void ResetRandoms()
        {
            Timer = 0;
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
            _randomSpawnPosIndex = Random.Range(0, _lanes.Count);
        }


        private void SpawnSpecialAttack()
        {

            if (_randomSpawnPosIndex < 0 || _lanes[_randomSpawnPosIndex] == null || _specialAttack == null) return;
            var specialAttack = Instantiate(_specialAttack.Prefab, _lanes[_randomSpawnPosIndex].MeteorRainSpawnPosition.transform.position, _lanes[_randomSpawnPosIndex].transform.localRotation);
            var behaviour = specialAttack.GetComponent<SpecialAttackBaseBehavior>();

            if (behaviour != null)
            {
                behaviour.Initialize(_specialAttack, _lanes[_randomSpawnPosIndex].MeteorRainSpawnPosition);
            }
            else
            {
                Debug.LogWarning("SpecialAttackBaseBehaviour not found on spawned enemy prefab.");
            }

        }

    }
}
