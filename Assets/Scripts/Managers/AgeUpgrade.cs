using BackEnd.Enums;
using Assets.Scripts.units;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;

namespace Managers
{
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
            unit.MoneyWhenKilled += levelUpData._moneyWhenKilled;

        }



    }
}

