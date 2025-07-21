using Assets.Scripts.BackEnd.Enems;
using UnityEngine;

namespace Assets.Scripts.BackEnd.Utilities
{
    public class SpriteEntries
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