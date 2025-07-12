using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InterFaces
{
    public interface IImgeSwichable<TType>
    {
        TType Type { get; }

    }
}