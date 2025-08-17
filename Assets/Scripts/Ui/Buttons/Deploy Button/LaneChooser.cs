using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using Managers.Spawners;
using UnityEngine;

namespace Ui.Buttons.Deploy_Button
{
    
    public class LaneChooser : SingletonMonoBehaviour<LaneChooser>
    {
        public bool choseLane;

        public void ChooseLane(UnitData unit, Vector3 spawnPos)
        {
            DeployManager.Instance.QueueUnitForDeployment(unit);
        }

        
    }
}