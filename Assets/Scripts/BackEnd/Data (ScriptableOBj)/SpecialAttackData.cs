using System.Collections.Generic;
using BackEnd.Enums;
using Assets.Scripts.Data;
using BackEnd.Base_Classes;
using UnityEngine;

namespace BackEnd.Data__ScriptableOBj_
{
    [CreateAssetMenu(fileName = "SpecialAttackData", menuName = "SpecialAttackData", order = 4)]
    public class SpecialAttackData : DataObject<SpecialAttackType>
    {

        public override void UpgradeAge(List<LevelUpDataBase> upgradeDataList)
        {
            foreach (var levelUpDataBase in upgradeDataList)
            {
                if (levelUpDataBase is not SpecialAttackLevelUpData levelUpData || Type != levelUpData.Type) continue;
                AgeStage = levelUpData.AgeStage;
                Prefab = levelUpData.Prefab;
            }
        }
    }
}