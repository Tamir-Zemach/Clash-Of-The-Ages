using System;
using System.Collections;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Special_Attacks
{
    public class MeteorRain : InGameObject
    {
        public event Action OnMeteorSpawned;

        [SerializeField, Tooltip("Prefab of the meteor object to be spawned during the attack.")]
        private GameObject _meteorPrefab;

        [SerializeField, Tooltip("The defined area within which meteors will randomly spawn.")]
        private Collider _specialAttackArea;

        [FormerlySerializedAs("_meteorSterngth")]
        [SerializeField, Tooltip("Strength For Meteor")]
        private int _meteorStrength;

        [SerializeField, Tooltip("Minimum delay (in seconds) between consecutive meteor spawns.")]
        private float _minSpawnTime;

        [SerializeField, Tooltip("Maximum delay (in seconds) between consecutive meteor spawns.")]
        private float _maxSpawnTime;

        private ManagedCoroutine _spawnRoutine;

        private void Awake()
        {
            StartSpawning();
        }

        private void StartSpawning()
        {
            _spawnRoutine = CoroutineManager.Instance.StartManagedCoroutine(SpawnCycle());
        }

        private Vector3 RandomSpawnPoint()
        {
            var areaMin = _specialAttackArea.bounds.min;
            var areaMax = _specialAttackArea.bounds.max;

            return new Vector3(
                Random.Range(areaMin.x, areaMax.x),
                Random.Range(areaMin.y, areaMax.y),
                Random.Range(areaMin.z, areaMax.z)
            );
        }

        private float RandomSpawnTime(float minSpawnTime, float maxSpawntime)
        {
            return Random.Range(minSpawnTime, maxSpawntime);
        }

        private IEnumerator SpawnCycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(RandomSpawnTime(_minSpawnTime, _maxSpawnTime));
                SpawnMeteorAtRandomPos();
            }
        }

        private void SpawnMeteorAtRandomPos()
        {
            var meteorReference = Instantiate(_meteorPrefab, RandomSpawnPoint(), Quaternion.identity);
            OnMeteorSpawned?.Invoke();

            var meteorScript = meteorReference.GetComponent<Meteor>();
            if (meteorScript != null)
            {
                meteorScript.SetStrength(_meteorStrength);
            }
        }

        #region GameLifecycle

        protected override void HandlePause()
        {
            _spawnRoutine?.Pause();
        }

        protected override void HandleResume()
        {
            _spawnRoutine?.Resume();
        }

        protected override void HandleGameEnd()
        {
            StopSpawning();
        }

        protected override void HandleGameReset()
        {
            StopSpawning();
        }

        private void StopSpawning()
        {
            _spawnRoutine?.Stop();
            _spawnRoutine = null;
        }

        #endregion
    }
}