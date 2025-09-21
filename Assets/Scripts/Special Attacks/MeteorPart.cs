
using System;
using UnityEngine;
using Special_Attacks;

namespace Special_Attacks
{
    public class MeteorPart : MonoBehaviour
    {
        private MeteorDestructible _parent;
        private Rigidbody _rb;

        private void Awake()
        {
            _parent = GetComponentInParent<MeteorDestructible>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _parent.OnImpact += ActiveRigidbody;
        }

        private void OnDestroy()
        {
            _parent.OnImpact -= ActiveRigidbody;
        }

        private void ActiveRigidbody(Vector3 impactPosition)
        {
            _rb.isKinematic = false;
            gameObject.transform.SetParent(null);
            _rb.AddExplosionForce(_parent.ExplosionForce, impactPosition, _parent.ExplosionForce);
            Destroy(gameObject, _parent.DebrisLifetime);
        }
    }
}