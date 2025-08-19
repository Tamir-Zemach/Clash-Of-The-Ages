using BackEnd.Data__ScriptableOBj_;
using BackEnd.InterFaces;
using Bases;
using units.Behavior;
using UnityEngine;

namespace units.Type
{

    public class Attacker : MonoBehaviour
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        private int _strength;
        private void Awake()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
            if (_unitBaseBehaviour != null)
            {
                _unitBaseBehaviour.OnAttack += Attack;
            }

        }
        private void OnDestroy()
        {
            if (_unitBaseBehaviour != null)
            {
                _unitBaseBehaviour.OnAttack -= Attack;
            }
        }
        public void Attack(GameObject target, int strength)
        {
            _strength = strength;
            GiveDamage(target); 
        }
        private void GiveDamage(GameObject target)
        {
            (target.GetComponent<UnitHealthManager>() as IDamageable
             ?? target.GetComponent<FriendlyBaseHealth>() as IDamageable
             ?? target.GetComponent<EnemyBaseHealth>() as IDamageable)
            ?.GetHurt(_strength);
        }

    }

}