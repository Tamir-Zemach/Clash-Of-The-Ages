

using UnityEngine;

namespace Assets.Scripts.InterFaces
{
    public interface IUpgradable<TType>
    {
        TType Type { get; }
        bool IsFriendly { get; }
        int AgeStage { get; }
        GameObject Prefab { get; }

        public void SetPrefab(GameObject prefab);
        public void SetType(TType type);

    }
}