using BackEnd.Enums;
using UnityEngine;

namespace BackEnd.InterFaces
{
    public interface IUpgradable<TType>
    {
        TType Type { get; }
        AgeStageType AgeStage { get; }
        bool IsFriendly { get; }
        GameObject Prefab { get; }
    }
}