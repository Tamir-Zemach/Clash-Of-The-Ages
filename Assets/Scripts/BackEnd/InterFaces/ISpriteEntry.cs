using UnityEngine;

namespace BackEnd.InterFaces
{
    public interface ISpriteEntry<TType>
    {
        TType GetKey();
        Sprite GetSprite();
    }
}