using System;
using Assets.Scripts.Enems;

namespace Assets.Scripts.BackEnd.Utilities
{
    // For unit upgrade and unit sprites — keyed by UnitType and StatType
    public struct UpgradeButtonSpriteKey : IEquatable<UpgradeButtonSpriteKey>
    {
        public UnitType UnitType;
        public StatType StatType;

        public UpgradeButtonSpriteKey(UnitType unitType, StatType statType)
        {
            UnitType = unitType;
            StatType = statType;
        }

        public bool Equals(UpgradeButtonSpriteKey other) =>
            UnitType == other.UnitType && StatType == other.StatType;

        public override bool Equals(object obj) =>
            obj is UpgradeButtonSpriteKey other && Equals(other);

        public override int GetHashCode() =>
            (int)UnitType * 397 ^ (int)StatType * 139;

        public override string ToString() =>
            $"Unit:{UnitType}, Stat:{StatType}";
    }


    //----------------------------------------------------

    // For Unit Sprite Map — keyed by UnitType
    public struct UnitSpriteKey : IEquatable<UnitSpriteKey>
    {
        public UnitType UnitType;

        public UnitSpriteKey(UnitType unitType)
        {
            UnitType = unitType;
        }

        public bool Equals(UnitSpriteKey other) =>
            UnitType == other.UnitType;

        public override bool Equals(object obj) =>
            obj is UnitSpriteKey other && Equals(other);

        public override int GetHashCode() =>
            (int)UnitType;

        public override string ToString() =>
            $"Unit:{UnitType}";
    }


    //----------------------------------------------------


    public struct SpecialAttackSpriteKey : IEquatable<SpecialAttackSpriteKey>
    {
        public AgeStageType AgeStageType;


        public SpecialAttackSpriteKey(AgeStageType agestageType)
        {
            AgeStageType = agestageType;
        }
        public bool Equals(SpecialAttackSpriteKey other) =>
            AgeStageType == other.AgeStageType;

        public override bool Equals(object obj) =>
            obj is SpecialAttackSpriteKey other && Equals(other);

        public override int GetHashCode() =>
            (int)AgeStageType;

    }

        //----------------------------------------------------

        public struct TurretSpriteKey : IEquatable<TurretSpriteKey>
        {
            public TurretType TurretType;

            public TurretSpriteKey(TurretType turretType)
            {
                TurretType = turretType;
            }

            public bool Equals(TurretSpriteKey other) =>
                TurretType == other.TurretType;

            public override bool Equals(object obj) =>
                obj is TurretSpriteKey other && Equals(other);

            public override int GetHashCode() =>
                (int)TurretType;
        }
}