using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InterFaces
{
    public interface IImgeSwichable<TType>
    {
        TType Type { get; }

        Sprite Sprite { get; }

        Image Image { get; }


        public void SetSprite(Sprite sprite);

    }
}