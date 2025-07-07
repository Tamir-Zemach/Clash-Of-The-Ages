using Assets.Scripts.Enems;
using UnityEngine;

namespace Assets.Scripts.BackEnd.Utilities
{
    public class SpriteEntries
    {
        [System.Serializable]
        public class UnitSpriteEntry : ISpriteEntry<UnitType>
        {
            public UnitType UnitType;
            public Sprite Sprite;

            public UnitType GetKey() => UnitType;
            public Sprite GetSprite() => Sprite;
        }

        [System.Serializable]
        public class UpgradeButtonSpriteEntry : ISpriteEntry<(UnitType, StatType)>
        {
            public UnitType UnitType;
            public StatType StatType;
            public Sprite Sprite;

            public (UnitType, StatType) GetKey() => (UnitType, StatType);
            public Sprite GetSprite() => Sprite;
        }

        [System.Serializable]
        public class SpecialAttackSpriteEntry : ISpriteEntry<AgeStageType>
        {
            public AgeStageType _ageStageType;
            public Sprite Sprite;

            public AgeStageType GetKey() => _ageStageType;
            public Sprite GetSprite() => Sprite;
        }

        [System.Serializable]
        public class TurretSpriteEntry : ISpriteEntry<(TurretType, TurretButtonType)>
        {
            public TurretType TurretType;
            public TurretButtonType TurretButtonType;
            public Sprite Sprite;

            public (TurretType, TurretButtonType) GetKey() => (TurretType, TurretButtonType);
            public Sprite GetSprite() => Sprite;
        }

    }

}