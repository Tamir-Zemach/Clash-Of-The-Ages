using BackEnd.Enums;
using BackEnd.InterFaces;
using UnityEngine;

namespace BackEnd.Base_Classes
{
    public abstract class LevelUpDataBase : ScriptableObject, ILevelUpData
    {
        [field: SerializeField] public AgeStageType AgeStage { get; private set; }
    }
}