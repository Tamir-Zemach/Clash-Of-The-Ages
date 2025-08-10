using BackEnd.Project_inspector_Addons;
using units.Behavior;
using UnityEngine;

namespace Special_Attacks
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField, TagSelector] private string _oppositeUnitTag;
        [SerializeField] private float _destroyTime;
        private float _timer;
        private int _damageStrength = 1;
        private bool _hasHit = false;


        public void SetStrength(int strength)
        {
            _damageStrength = strength;
        }


        private void OnTriggerEnter(Collider other)
        {
            //check for hit one time
            if (_hasHit) return;

            if (other.gameObject.CompareTag(_oppositeUnitTag))
            {
                _hasHit = true;
                GiveDamage(other.gameObject);
                Destroy(gameObject);
            }
        }
        private void GiveDamage(GameObject target)
        {
            UnitHealthManager targetHealth = target.GetComponent<UnitHealthManager>();
            if (targetHealth != null)
            {
                targetHealth.GetHurt(_damageStrength);
            }
        }

        private void Update()
        {
            DestroyAfterCertainTime();
        }
        private void DestroyAfterCertainTime()
        {
            _timer += Time.deltaTime;
            if (_timer >= _destroyTime)
            {
                Destroy(gameObject);
            }
        }

    }
}
