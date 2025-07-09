using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] float _fadeOutDuration = 1;
    [SerializeField] float _initialdelayForfadeOut = 2;

    private Slider slider;
    private UnitHealthManager healthManager;
    private CanvasGroup CanvasGroup;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        healthManager = GetComponentInParent<UnitHealthManager>();
        CanvasGroup = GetComponentInParent<CanvasGroup>();
        CanvasGroup.alpha = 0f;
    }

    private void Start()
    {
        SetMaxHealth(healthManager.currentHealth);
        healthManager.OnHealthChanged += SetHealth;
    }
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }


    private void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    private void SetHealth()
    {
        slider.value = healthManager.currentHealth;
        DisplayHealthBar();
        FadeOutHealthBar();
    }

    private void DisplayHealthBar() => CanvasGroup.alpha = 1f;

    private void FadeOutHealthBar()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(_fadeOutDuration, _initialdelayForfadeOut));
    }

    private IEnumerator FadeOutCoroutine(float duration, float initialdelay)
    {    
        yield return new WaitForSeconds(initialdelay);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            CanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

}
