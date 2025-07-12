

using Assets.Scripts.Enems;
using UnityEngine;

namespace Assets.Scripts.InterFaces
{
    public interface IUpgradable<TType>
    {
        TType Type { get; }
        AgeStageType AgeStage { get; }
        bool IsFriendly { get; }
        GameObject Prefab { get; }
    }
}