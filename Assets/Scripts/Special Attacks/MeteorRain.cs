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
        private BoxCollider _specialAttackArea;

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
            CameraShake.Shake();
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
            if (!_isDestroyed && _specialAttackArea != null)
            {
                return RandomPointInBoxCollider(_specialAttackArea);
            }

            Debug.LogWarning($"{name}: Invalid spawn area — using fallback position.");
            return transform != null ? transform.position : Vector3.zero;
        }
        private Vector3 RandomPointInBoxCollider(BoxCollider box)
        {
            // Random point within the full size of the box
            Vector3 localPoint = new Vector3(
                Random.Range(0f, box.size.x),
                Random.Range(0f, box.size.y),
                Random.Range(0f, box.size.z)
            );

            // Shift to center-based coordinates
            localPoint -= box.size * 0.5f;

            // Offset by the collider's center
            localPoint += box.center;

            // Convert to world space
            return box.transform.TransformPoint(localPoint);
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

        public override void HandlePause()
        {
            _spawnRoutine?.Pause();
        }

        public override void HandleResume()
        {
            _spawnRoutine?.Resume();
        }

        public override void HandleGameEnd()
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