using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public bool IsFriendly;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
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
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    private void SetHealth()
    {
        slider.value = IsFriendly ? PlayerHealth.Instance.CurrentHealth : EnemyHealth.Instance.CurrentHealth;
    }
}
