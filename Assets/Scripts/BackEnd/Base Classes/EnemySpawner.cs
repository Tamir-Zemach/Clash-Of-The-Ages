using UnityEngine;
using UnityEngine.Serialization;

namespace BackEnd.Base_Classes
{
    public abstract class EnemySpawner<T> : SceneAwareMonoBehaviour<T> where T : MonoBehaviour
    {
        [Tooltip("Minimum time interval before a prefab can spawn (can be a decimal value).")] 
        [field: SerializeField] public float MinSpawnTime { get; set; }

        [Tooltip("Maximum time interval before a prefab can spawn (can be a decimal value).")]  
        [field: SerializeField] public float MaxSpawnTime { get; set; }

        protected abstract float RandomSpawnTimer { get; set; }
        protected abstract float Timer { get; set; }

        protected abstract bool CanDeploy();
    }
}