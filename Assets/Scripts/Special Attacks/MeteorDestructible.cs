using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Special_Attacks
{
    public class MeteorDestructible : MonoBehaviour
    {
        public Action<Vector3> OnImpact;

        [Header("Fall Direction Settings")] 
        [Tooltip("The direction which the meteor will go toward, on the local Z axis.")]
        [Range(-1f, 1f)]
        public float FallDirectionZ = 0;
        [Tooltip("Extra downward force applied to the meteor. 0 = normal gravity. Higher values make it fall faster.")]
        public float AdditionalFallForce = 100f;
    
        [Header("Destruction Settings")]
        [Tooltip("How long (in seconds) the broken pieces will last before disappearing.")]
        public float DebrisLifetime = 5.0f;

        [Tooltip("The force applied to the pieces when the meteor breaks.")]
        public float ExplosionForce = 700f;

        [Tooltip("The radius of the explosion effect.")]
        public float ExplosionRadius = 10f;
    
    
        private Rigidbody _rb;
        private bool _hasCollided = false;
    
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
    
        private void FixedUpdate()
        {
            if (_hasCollided || AdditionalFallForce <= 0) return;
            
            
            var worldDirection = transform.TransformDirection(SetRandomDirection().normalized);
            
            _rb.AddForce(worldDirection * AdditionalFallForce);
        }

        private Vector3 SetRandomDirection()
        {
            return new Vector3(0, -1f, Random.Range(0, FallDirectionZ));
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (_hasCollided) return;

            _hasCollided = true;

            Vector3 impactPoint = transform.position; // fallback

            // Check if collider type supports ClosestPoint
            if (IsClosestPointSafe(other))
            {
                impactPoint = other.ClosestPoint(transform.position);
            }

            OnImpact?.Invoke(impactPoint);
            Destroy(gameObject);
        }
        
        private bool IsClosestPointSafe(Collider col)
        {
            return col is BoxCollider || col is SphereCollider || col is CapsuleCollider ||
                   (col is MeshCollider mesh && mesh.convex);
        }
    }
}