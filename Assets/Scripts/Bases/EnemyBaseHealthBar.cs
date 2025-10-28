using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bases
{
    public class EnemyBaseHealthBar :  MonoBehaviour
    {
        private EnemyBaseHealth _enemyBaseHealth;
        private Slider _slider;
        private void Awake()
        {
            _enemyBaseHealth = GetComponentInParent<EnemyBaseHealth>();
            _slider = GetComponent<Slider>();
        }
        
        private void Start()
        {
            _enemyBaseHealth.OnMaxHealthChanged += SetMaxHealth;
            _enemyBaseHealth.OnHealthChanged += SetHealth;
        }

        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
        
        private void SetMaxHealth(int maxHealth)
        {
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
            print(maxHealth);
        }

        private void SetHealth()
        {
            _slider.value = _enemyBaseHealth.CurrentHealth;
        }
        
    }
}