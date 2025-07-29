using System;
using BackEnd.Data__ScriptableOBj_;
using units.Behavior;
using units.Type;
using UnityEngine;
using Random = UnityEngine.Random;
using Range = units.Type.Range;

namespace Animations
{
    public class UnitAnimationActions : MonoBehaviour
    {
        public event Action OnAttack;
        private GameObject _parent;
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitData _unitData;

        private void Awake()
        {
            _unitBaseBehaviour = GetComponentInParent<UnitBaseBehaviour>();
            _parent = _unitBaseBehaviour.gameObject;
           
        }

        private void Start()
        {
            _unitData = _unitBaseBehaviour.Unit;
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
            var randomStrength = SetRandomStrength();
            GameObject target = _unitBaseBehaviour.GetAttackTarget();
            if (target == null) return;

            Attacker attacker = _unitBaseBehaviour.GetComponent<Attacker>();
            if (attacker != null)
            {
                attacker.Attack(target, randomStrength);
                //print($"{gameObject.name} attacked with strength: {randomStrength}");
                OnAttack?.Invoke();
                return;
            }
            else
            {
                Range ranger = _unitBaseBehaviour.GetComponent<Range>();
                if (ranger != null)
                {
                    //print($"{gameObject.name} attacked with strength: {randomStrength}");
                    ranger.FireProjectile(target, randomStrength);
                    OnAttack?.Invoke();
                }
            }

        }
        
                    
        private int SetRandomStrength()
        {
            return Random.Range(_unitData.MinStrength,  _unitData.MaxStrength);
        }
        
    }
}
