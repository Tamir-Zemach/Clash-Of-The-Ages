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

        private bool _isDestroyed = false;

        private GameObject _enemyBase;
        
        private void Awake()
        {
            StartSpawning();
        }

        private void StartSpawning()
        {
            _spawnRoutine = CoroutineManager.Instance.StartManagedCoroutine(SpawnCycle());
        }


        private void OnDestroy()
        {
            _isDestroyed = true;
            StopSpawning();
        }

        private Vector3 RandomSpawnPoint()
        {
            if (!_isDestroyed && _specialAttackArea != null && _specialAttackArea.enabled)
                return RandomPointInCollider(_specialAttackArea);
            Debug.LogWarning($"{name}: Invalid spawn area — using fallback position.");
            return transform != null ? transform.position : Vector3.zero;

        }
        private Vector3 RandomPointInCollider(Collider col)
        {
            Vector3 point;
            int attempts = 0;
            do
            {
                point = new Vector3(
                    Random.Range(col.bounds.min.x, col.bounds.max.x),
                    Random.Range(col.bounds.min.y, col.bounds.max.y),
                    Random.Range(col.bounds.min.z, col.bounds.max.z)
                );
                attempts++;
            } while (!col.bounds.Contains(point) || !IsPointInsideCollider(col, point) && attempts < 10);

            return point;
        }

        private bool IsPointInsideCollider(Collider col, Vector3 point)
        {
            Vector3 closest = col.ClosestPoint(point);
            return Vector3.Distance(closest, point) < 0.01f;
        }

        private float RandomSpawnTime(float minSpawnTime, float maxSpawntime)
        {
            return Random.Range(minSpawnTime, maxSpawntime);
        }

        private IEnumerator SpawnCycle()
        {
            while (!_isDestroyed)
            {
                yield return new WaitForSeconds(RandomSpawnTime(_minSpawnTime, _maxSpawnTime));
                SpawnMeteorAtRandomPos();
            }
        }

        private void SpawnMeteorAtRandomPos()
        {
            if (_meteorPrefab == null)
            {
                Debug.LogWarning($"{name}: Meteor prefab is missing.");
                return;
            } 
            

            var meteorReference = Instantiate(_meteorPrefab, RandomSpawnPoint(), Quaternion.identity);
            if (meteorReference == null) return;

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