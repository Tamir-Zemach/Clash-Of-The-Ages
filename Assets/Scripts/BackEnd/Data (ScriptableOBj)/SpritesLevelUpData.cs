using System.Collections.Generic;
using BackEnd.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using static BackEnd.Utilities.SpriteEntries;
using static BackEnd.Utilities.SpriteKeys;

namespace BackEnd.Data__ScriptableOBj_
{
    public class SpritesLevelUpData : LevelUpDataBase
    {
        #region GeneralSprites
        public List<SpriteEntry<UnitType>> UnitSpriteMap;
        public List<SpriteEntry<SpecialAttackType>> SpecialAttackSpriteMap;
        [FormerlySerializedAs("turretSpriteMap")] public List<SpriteEntry<TurretKey>> TurretSpriteMap;
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
}

