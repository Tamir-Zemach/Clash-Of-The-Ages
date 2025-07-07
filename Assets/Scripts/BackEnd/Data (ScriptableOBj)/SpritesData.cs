
using Assets.Scripts.BackEnd.Utilities;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Backend.Data
{
    [CreateAssetMenu(fileName = "SpritesData", menuName = "SpritesData", order = 7)]
    public class SpritesData : ScriptableObject
    {

        [SerializeField] private AgeStageType _currentAgeStage;

        public AgeStageType AgeStage => _currentAgeStage;


        public List<SpriteEntries.UpgradeButtonSpriteEntry> unitUpgradeButtonSpriteMap;

        public List<SpriteEntries.UnitSpriteEntry> unitSpriteMap;

        public List<SpriteEntries.SpecialAttackSpriteEntry> specialAttackSpriteMap;

        public List<SpriteEntries.TurretSpriteEntry> turretSpriteMap;

        public Sprite GetSpriteFromList<TType, TEntry>(List<TEntry> spriteEntries, TType type) where TEntry : ISpriteEntry<TType>
        {
            foreach (var entry in spriteEntries)
            {
                if (EqualityComparer<TType>.Default.Equals(entry.GetKey(), type))
                    return entry.GetSprite();
            }
            return null;
        }
        public List<Sprite> GetAllSpritesFromMap<TEntry, TType>(List<TEntry> spriteEntries) where TEntry : ISpriteEntry<TType>
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

    }

}
