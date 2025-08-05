using BackEnd.Enums;

namespace BackEnd.Utilities
{
    public struct SpriteKeys
    {
        [System.Serializable]
        public struct UnitUpgradeButtonKey
        {
            public UnitType UnitType;
            public StatType StatType;
        }

        [System.Serializable]
        public struct TurretKey
        {
            public TurretButtonType ButtonType;
            public TurretType TurretType;
        }
    }

}