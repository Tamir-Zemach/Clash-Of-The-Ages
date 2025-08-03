using BackEnd.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class HealthBar : MonoBehaviour
    {
        public bool IsFriendly;
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            if (IsFriendly)
            {
                PlayerHealth.OnHealthChanged += SetHealth;
            }
            else
            {
                EnemyHealth.OnEnemyHealthChanged += SetHealth;
            }
        }

        private void Start()
        {
            SetMaxHealth(IsFriendly ? PlayerHealth.Instance.MaxHealth : EnemyHealth.Instance.MaxHealth);
        }

        private void SetMaxHealth(int maxHealth)
        {
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
        }

        private void SetHealth()
        {
            _slider.value = IsFriendly ? PlayerHealth.Instance.CurrentHealth : EnemyHealth.Instance.CurrentHealth;
        }
    }
}
