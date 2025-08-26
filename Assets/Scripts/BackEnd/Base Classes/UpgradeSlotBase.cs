using BackEnd.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace BackEnd.Base_Classes
{
    public abstract class UpgradeSlotBase : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;

        public Sprite Icon => _iconImage != null ? _iconImage.sprite : null;
        
        public abstract SlotType SlotType { get; }
        
#if UNITY_EDITOR
        public static class FieldNames
        {
            public const string Image = nameof(_iconImage);
        }
#endif   
    }
}