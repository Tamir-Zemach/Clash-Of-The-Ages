using Assets.Scripts.Enems;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.BackEnd.Utilities.SpriteEntries;

public class SpritesLevelUpData : LevelUpDataBase
{
    #region GeneralSprites
    public List<SpriteEntry<UnitType>> UnitSpriteMap;
    public List<SpriteEntry<SpecialAttackType>> SpecialAttackSpriteMap;
    [System.Serializable]
    public struct TurretKey { public TurretButtonType ButtonType; public TurretType TurretType;  }
    public List<SpriteEntry<TurretKey>> turretSpriteMap;

    [System.Serializable]
    public struct UnitUpgradeButtonKey { public UnitType UnitType; public StatType StatType; }
    public List<SpriteEntry<UnitUpgradeButtonKey>> UnitUpgradeButtonSpriteMap;

    public Sprite GetSpriteFromList<TType>(TType type, List<SpriteEntry<TType>> spriteEntries)
    {
        foreach (var entry in spriteEntries)
        {
            if (EqualityComparer<TType>.Default.Equals(entry.GetKey(), type))
                return entry.GetSprite();
        }
        return null;
    }

    public List<Sprite> GetAllSpritesFromMap<TType>(List<SpriteEntry<TType>> spriteEntries)
    {
        var sprites = new List<Sprite>();
        foreach (var entry in spriteEntries)
        {
            var sprite = entry.GetSprite();
            if (sprite != null)
                sprites.Add(sprite);
        }
        return sprites;
    }
    #endregion


}



#if UNITY_EDITOR
public static class FieldNames
{
    public const string UnitSpriteMap = nameof(UnitSpriteMap);
    public const string UnitUpgradeButtonSpriteMap = nameof(UnitUpgradeButtonSpriteMap);
    public const string SpecialAttackSpriteMap = nameof(SpecialAttackSpriteMap);
    public const string TurretSpriteMap = nameof(TurretSpriteMap);
}
#endif