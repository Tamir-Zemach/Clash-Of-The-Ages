using Assets.Scripts;
using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminUI : PersistentMonoBehaviour<AdminUI>
{
    [Header("Panels")]
    public GameObject AdminPanel;

    [Header("Currency Controls")]
    public TMP_InputField moneyToAddField;
    public TMP_InputField moneyToSubtractField;

    [Header("Health Controls")]
    public TMP_InputField healthToAddField;

    [Header("Game Speed")]
    public Slider gameSpeedSlider;

    [Header("Toggles")]
    public Toggle easyModeToggle;

    [Header("Unit Info Displays")]
    public TMP_Text friendlyUnitText;
    public TMP_Text enemyUnitText;
    public TMP_Text unitCounterText;

    protected override void Awake()
    {
        base.Awake();
        AdminPanel.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowPanel(AdminPanel);
        }
        DisplayParams();
    }
    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void AddMoney()
    {
        if (int.TryParse(moneyToAddField.text, out int amount))
        {
            PlayerCurrency.Instance.AddMoney(amount);
        }
        else
        {
            moneyToAddField.text = "0";
        }
    }

    public void SubtractMoney()
    {
        if (int.TryParse(moneyToSubtractField.text, out int amount))
        {
            PlayerCurrency.Instance.SubtractMoney(amount);
        }
        else
        {
            moneyToSubtractField.text = "0";
        }
    }

    public void AddHealth()
    {
        if (int.TryParse(healthToAddField.text, out int amount))
        {
            PlayerHealth.Instance.AddHealth(amount);
        }
        else
        {
            healthToAddField.text = "0";
        }
    }

    public void ApplyGameSpeed()
    {
        float speed = gameSpeedSlider.value;
        Time.timeScale = speed;
    }

    public void EasyMode()
    {
        Admin.Instance._easyMode = easyModeToggle.isOn;
    }

    public void UpgradeAge()
    {
        Admin.Instance.UpgradeAgeToLevel();
    }

    private void DisplayParams()
    {
        DisplayEnemyUnitParameters();
        DisplayFriendlyUnitParameters();
        DisplayUnitCounter();
    }

    public void DisplayFriendlyUnitParameters()
    {
        var info = "";
        foreach (var unit in GameDataRepository.Instance.FriendlyUnits)
        {
            info += $"{unit.name}\n" +
                    $"Cost: {unit.Cost}, Health: {unit.Health}, Speed: {unit.Speed}, Strength: {unit.Strength}\n" +
                    $"Attack Speed: {unit.InitialAttackDelay}, Range: {unit.Range}\n\n";
        }
        friendlyUnitText.text = info;
    }

    public void DisplayEnemyUnitParameters()
    {
        var info = "";
        foreach (var unit in GameDataRepository.Instance.EnemyUnits)
        {
            info += $"{unit.name}\n" +
                    $"Reward: {unit._moneyWhenKilled}, Health: {unit.Health}, Speed: {unit.Speed}, Strength: {unit.Strength}\n" +
                    $"Attack Speed: {unit.InitialAttackDelay}, Range: {unit.Range}\n\n";
        }
        enemyUnitText.text = info;
    }

    public void DisplayUnitCounter()
    {
        unitCounterText.text = $"Friendly Units: {UnitCounter.FriendlyCount}\n" +
                               $"Enemy Units: {UnitCounter.EnemyCount}";
    }

}