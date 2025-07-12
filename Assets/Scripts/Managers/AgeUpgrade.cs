
using Assets.Scripts.Enems;
using Assets.Scripts.units;


public class AgeUpgrade : PersistentMonoBehaviour<AgeUpgrade>
{

    public AgeStageType CurrentPlayerAge { get; private set; } = AgeStageType.StoneAge;
    public AgeStageType CurrentEnemyAge { get; private set; } = AgeStageType.StoneAge;

    public void AdvanceAge(bool isFriendly)
    {
        if (isFriendly) CurrentPlayerAge++;
        else CurrentEnemyAge++;
    }

    //TODO: Enemy AgeUpgrade
    public void UpdateUnitReward(UnitData unit, UnitLevelUpData levelUpData)
    {
        unit._moneyWhenKilled += levelUpData._moneyWhenKilled;

    }



}

