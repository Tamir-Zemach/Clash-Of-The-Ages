using BackEnd.Enums;
using Assets.Scripts.InterFaces;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelUpDataBase : ScriptableObject, ILevelUpData
{
    [field: SerializeField] public AgeStageType AgeStage { get; private set; }
}