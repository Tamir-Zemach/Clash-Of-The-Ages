using System;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Economy;
using BackEnd.InterFaces;
using UnityEngine;
using VisualCues;

namespace units.Behavior
{
    public class UnitHealthManager : MonoBehaviour, IDamageable
    {
        public event Action OnHealthChanged;
        public event Action OnDying;
        private UnitData _unit;
        private UnitBaseBehaviour _unitBaseBehaviour;
        private int _currentHealth;
        private Animator _animator;
        private bool _isDying;

        public int CurrentHealth => _currentHealth;

        private void Start()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();

            _unit = _unitBaseBehaviour.Unit;
            _currentHealth = _unit.Health;
        }

        public void GetHurt(int damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke();

            if (_currentHealth > 0 || _isDying) return;
            _isDying = true;
            Die();
        }

        private void Die()
        {
            if (!_unit.IsFriendly)
            {
                CoinPickupEffect.Instance.SpawnCoins(gameObject.transform.position, () =>
                {
                    PlayerCurrency.Instance.AddMoney(_unit.MoneyWhenKilled);
                    PlayerExp.Instance.AddExp(_unit.ExpWhenKilled);
                });
            }

            TrygetAnimator();
            if (_animator != null)
            {
                OnDying?.Invoke();
            }
            else
            {
                OnDying?.Invoke();
                Destroy(gameObject);
            }
        }

        private void TrygetAnimator()
        {
            _animator = GetComponentInChildren<Animator>();
        }

    }
}





