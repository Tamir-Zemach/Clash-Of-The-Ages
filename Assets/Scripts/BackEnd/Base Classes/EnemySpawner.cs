using BackEnd.InterFaces;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace BackEnd.Base_Classes
{
    public abstract class EnemySpawner<T> : SceneAwareMonoBehaviour<T>,
        IGameStateListener where T : MonoBehaviour
    {
        [Tooltip("Minimum time interval before a prefab can spawn (can be a decimal value).")] 
        [field: SerializeField] public float MinSpawnTime { get; set; }

        [Tooltip("Maximum time interval before a prefab can spawn (can be a decimal value).")]  
        [field: SerializeField] public float MaxSpawnTime { get; set; }

        protected abstract float RandomSpawnTimer { get; set; }
        protected abstract float Timer { get; set; }

        protected abstract bool CanDeploy();

        protected void ResetAndReRandomTimer()
        {
            RandomSpawnTimer = Random.Range(MinSpawnTime, MaxSpawnTime);
            Timer = 0;
        }

        
        protected override void OnEnable()
        {
            base.OnEnable();
            GameStateListener.Subscribe(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameStateListener.Unsubscribe(this);
        }
        
        public abstract void HandlePause();
        public abstract void HandleResume();
        public abstract void HandleGameEnd();

        
        
    }
}