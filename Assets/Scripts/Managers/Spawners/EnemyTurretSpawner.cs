using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using Managers.Loaders;
using turrets;
using UnityEngine;

namespace Managers.Spawners
{
    public class EnemyTurretSpawner : EnemySpawner<EnemyTurretSpawner>
    {
        public System.Action OnTurretPlaced;

        protected override float RandomSpawnTimer { get; set; }
        protected override float Timer { get; set; }

        private TurretSpawnPoint _availableSpawnPoint;

        private List<TurretData> _enemyTurrets;

        private bool _isTurretWaitingToSpawn = false;

        private GameObject _playerBase;
        protected override void Awake()
        {
            base.Awake();
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
        }

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _enemyTurrets = GameDataRepository.Instance.EnemyTurrets;
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
            EnemyTurretSlotSpawner.Instance.OnTurretSlotActivated += StartTimer;
            ResetAndReRandomTimer();
            var turret = _enemyTurrets[0];
            _playerBase = GameObject.FindGameObjectWithTag(turret.OppositeBase);
        }



        private void StartTimer(TurretSpawnPoint slot)
        {
            _isTurretWaitingToSpawn = true;
            _availableSpawnPoint = slot;
            ResetAndReRandomTimer();
        }

        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu() || !GameStates.Instance.GameIsPlaying) return;
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

            var randomTurretData = _enemyTurrets[Random.Range(0, _enemyTurrets.Count)];

            var enemyReference = Instantiate(
                randomTurretData.Prefab,
                _availableSpawnPoint.transform.position,
                _availableSpawnPoint.transform.rotation
            );
            enemyReference.transform.SetParent(_availableSpawnPoint.transform);
            var behaviour = enemyReference.GetComponent<TurretBaseBehavior>();

            if (behaviour)
            {
                behaviour.Initialize(randomTurretData, _availableSpawnPoint.transform, _playerBase);
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
        

        public override void HandlePause()
        {
          
        }

        public override void HandleResume()
        {
            
        }

        public override void HandleGameEnd()
        {
            ResetAndReRandomTimer();
        }


    }
}
