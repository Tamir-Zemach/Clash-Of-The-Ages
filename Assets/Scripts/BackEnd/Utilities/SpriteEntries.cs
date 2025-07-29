using UnityEngine;

namespace BackEnd.Utilities
{
    public abstract class SpriteEntries
    {
        [System.Serializable]
        public class SpriteEntry<TType> : ISpriteEntry<TType>
        {
            [SerializeField] private TType _key;
            [SerializeField] private Sprite _sprite;

            public TType GetKey() => _key;
            public Sprite GetSprite() => _sprite;
        }
    }


}