using UnityEngine;

namespace Assets.Scripts.BackEnd.BaseClasses
{
    public abstract class EnemySpawner<T> : SceneAwareMonoBehaviour<T> where T : MonoBehaviour
    {
        [Tooltip("Minimum time interval before a prefab can spawn (can be a decimal value).")]
        [field: SerializeField] public float _minSpawnTime { get; private set; }

        [Tooltip("Maximum time interval before a prefab can spawn (can be a decimal value).")]
        [field: SerializeField] public float _maxSpawnTime { get; private set; }

        protected abstract float RandomSpawnTimer { get; set; }
        protected abstract float Timer { get; set; }

        protected abstract bool CanDeploy();
    }
}