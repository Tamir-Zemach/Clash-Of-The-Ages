
using Assets.Scripts.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAgeManager : SceneAwareMonoBehaviour<EnemyAgeManager>
{
    public delegate void AgeUpgradeDelegate(List<LevelUpDataBase> data);
    public event AgeUpgradeDelegate OnAgeUpgrade;
    private List<LevelUpDataGroup> _levelUpData;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        if (GameDataRepository.Instance.EnemyLevelUpData != null)
        {
            _levelUpData = GameDataRepository.Instance.EnemyLevelUpData;
        }
    }


    public void UpgradeEnemyAge()
    {
        AgeUpgrade.Instance.AdvanceAge(isFriendly: false);
        EnemyHealth.Instance.FullHealth();

        var dataGroup = _levelUpData.FirstOrDefault(g => g.AgeStage == AgeUpgrade.Instance.CurrentEnemyAge);
        if (dataGroup != null)
        {
            OnAgeUpgrade?.Invoke(dataGroup.LevelUpEntries);
        }
        else
        {
            Debug.LogWarning($"No LevelUpDataGroup found for AgeStage: {AgeUpgrade.Instance.CurrentEnemyAge}");
        }
    }


}
