using System;
using Assets.Scripts;
using UnityEngine;

namespace Animations
{
    public class UnitAnimationActions : MonoBehaviour
    {
        public event Action OnAttack;
        private GameObject _parent;
        private UnitBaseBehaviour _unitBaseBehaviour;

        private void Awake()
        {
            _unitBaseBehaviour = GetComponentInParent<UnitBaseBehaviour>();
            _parent = _unitBaseBehaviour.gameObject;
        }
        public void DestroyObject()
        {
            if (_parent != null)
            {
                Destroy(_parent.gameObject);
            }
        }

        public void AttackInvoke()
        {
            GameObject target = _unitBaseBehaviour.GetAttackTarget();
            if (target == null) return;

            Attacker attacker = _unitBaseBehaviour.GetComponent<Attacker>();
            if (attacker != null)
            {
                attacker.Attack(target);
                OnAttack?.Invoke();
                return;
            }
            else
            {
                Range ranger = _unitBaseBehaviour.GetComponent<Range>();
                if (ranger != null)
                {
                    ranger.FireProjectile(target);
                    OnAttack?.Invoke();
                }
            }

        }
    }
}
