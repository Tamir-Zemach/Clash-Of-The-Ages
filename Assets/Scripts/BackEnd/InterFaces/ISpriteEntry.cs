using UnityEngine;

public interface ISpriteEntry<TType>
{
    TType GetKey();
    Sprite GetSprite();
}