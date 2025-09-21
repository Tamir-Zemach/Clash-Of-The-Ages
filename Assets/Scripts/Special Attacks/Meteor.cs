using BackEnd.Project_inspector_Addons;
using units.Behavior;
using UnityEngine;

namespace Special_Attacks
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField, TagSelector] private string _oppositeUnitTag;
        private int _damageStrength = 1;
        private bool _hasHit = false;


        public void SetStrength(int strength)
        {
            _damageStrength = strength;
        }


        private void OnTriggerEnter(Collider other)
        {

            if (_hasHit) return;

            if (other.gameObject.CompareTag(_oppositeUnitTag))
            {
                _hasHit = true;
                GiveDamage(other.gameObject);
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

    }
}
