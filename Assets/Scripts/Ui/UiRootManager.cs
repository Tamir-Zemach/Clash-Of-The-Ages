
using UnityEngine.SceneManagement;
using Assets.Scripts.Backend.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.Ui.TurretButton;
using UnityEngine;
using System;
using Assets.Scripts.InterFaces;

public class UIRootManager : SceneAwareMonoBehaviour<UIRootManager>
{
    public event Action OnSceneChanged;
    private UnitDeployButton[] _unitDeployButtons;
    private UnitUpgradeButton[] _unitUpgradeButtons;
    private TurretButton[] _turretButtons;
    private SpecialAttackButton _specialAttackButton;

    protected override void Awake()
    {
        base.Awake();  
        FindUIButtons();
        GameManager.Instance.OnAgeUpgrade += SetSpritesToButtons;
    }


    protected override void InitializeOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        ResetaAlphaInAllCanvasGroups();
        OnSceneChanged?.Invoke();
    }

    private void ResetaAlphaInAllCanvasGroups()
    {
        CanvasGroup[] canvasGroups = GetComponentsInChildren<CanvasGroup>();

        foreach (var group in canvasGroups)
        {
            if (group != null)
                group.alpha = 0;
        }
    }

    private void FindUIButtons()
    {
        _unitDeployButtons = UIObjectFinder.GetButtons<UnitDeployButton, UnitType>();
        _unitUpgradeButtons = UIObjectFinder.GetButtons<UnitUpgradeButton, UnitType>();
        _turretButtons = UIObjectFinder.GetButtons<TurretButton, TurretType>();
        _specialAttackButton = UIObjectFinder.GetButton<SpecialAttackButton, AgeStageType>();
    }

    private void SetSpritesToButtons(ILevelUpData data)
    {
        if (data is SpritesLevelUpData levelUpData)
        {
            foreach (var button in _unitDeployButtons)
            {
                var sprite = levelUpData.GetSpriteFromList(button.Type, levelUpData.unitSpriteMap);
                if (sprite != null)
                button.SetSprite(sprite);
            }
            foreach (var button in _turretButtons)
            {
                var sprite = levelUpData.GetSpriteFromList((button.Type, button.ButtonType), levelUpData.turretSpriteMap);
                if (sprite != null)
                button.SetSprite(sprite);
            }
            foreach (var button in _unitUpgradeButtons)
            {
                var sprite = levelUpData.GetSpriteFromList((button.Type, button.StatType), levelUpData.unitUpgradeButtonSpriteMap);
                if (sprite != null)
                button.SetSprite(sprite);
            }
            _specialAttackButton.SetSprite(levelUpData.GetSpriteFromList(_specialAttackButton.Type, levelUpData.specialAttackSpriteMap));

        }
    }

}