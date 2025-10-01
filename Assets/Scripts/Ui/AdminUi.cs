using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Economy;
using BackEnd.Project_inspector_Addons;
using Managers;
using Managers.Loaders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class AdminUI : SingletonMonoBehaviour<AdminUI>
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
        

        [Header("Level Loader")]
        public TMP_Dropdown levelDropdown;

        [field: SerializeField, TagSelector] public string EnemyUnitTag;
        protected override void Awake()
        {
            base.Awake();
            AdminPanel.SetActive(false);
            PopulateLevelDropdown();
        }

        private void PopulateLevelDropdown()
        {
            List<string> options = new();

            foreach (var scene in LevelLoader.Instance.SceneList)
            {
                options.Add(scene.GetSceneName()); 
            }

            levelDropdown.ClearOptions();
            levelDropdown.AddOptions(options);
        }

        public void LoadLevelFromDropdown()
        {
            int selectedIndex = levelDropdown.value;
            LevelLoader.Instance.LoadSpecificLevel(selectedIndex);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ShowPanel(AdminPanel);
            }
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

        public void InfiniteHealth()
        {
            PlayerHealth.Instance.SetMaxHealth(999999999);
            PlayerHealth.Instance.FullHealth();
        }
        public void FullHealth()
        {
            PlayerHealth.Instance.FullHealth();
        }

        public void ApplyGameSpeed()
        {
            float speed = gameSpeedSlider.value;
            Time.timeScale = speed;
        }

        public void UpgradeAge()
        {
            GameManager.Instance.UpgradePlayerAge();
        }

        public void WipeOutAllEnemies()
        {
            List<GameObject> enemies = GameObject.FindGameObjectsWithTag(EnemyUnitTag).ToList();
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }

        }
        

        public void LevelUp()
        {
           PlayerExp.Instance.LevelUp();
        }



    }
}