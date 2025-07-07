using System;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.InterFaces;

[RequireComponent(typeof(UnitBaseBehaviour))]
public class UnitHealthManager : MonoBehaviour, IDamageable
{
    public event Action OnHealthChanged;
    public event Action OnDying;
    private UnitData Unit;
    private UnitBaseBehaviour UnitBaseBehaviour;
    private int _currentHealth;
    private Animator _animator;
    private bool isDying;
    private void Start()
    {
        UnitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
        Unit = UnitBaseBehaviour.Unit;
        _currentHealth = Unit.Health;
    }

    public void GetHurt(int damage)
    {
        _currentHealth -= damage;

        OnHealthChanged?.Invoke();

        if (_currentHealth <= 0 && !isDying)
        {
            isDying = true;
            Die();
        }
    }

    private void Die()
    {
        PlayerCurrency.Instance.AddMoney(Unit._moneyWhenKilled);
        TrygetAnimator();
        if (_animator != null)
        {
            OnDying?.Invoke();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void TrygetAnimator()
    {
       _animator = GetComponentInChildren<Animator>();
    }

}





