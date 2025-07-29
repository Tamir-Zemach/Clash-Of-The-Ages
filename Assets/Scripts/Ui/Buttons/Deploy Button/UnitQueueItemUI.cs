
using Assets.Scripts.BackEnd.Enems;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Buttons.Deploy_Button
{
    public class UnitQueueItemUI : MonoBehaviour, IImageSwitchable<UnitType>
    {
        [field: SerializeField] public UnitType  Type { get; set; }
        
        private Slider _fillBar;
        private CanvasGroup _canvasGroup;
        private float _deployDelay;

        public bool HasStarted { get; private set; }
        public int ID { get; set; }
        public Image Image {get; set;}
        
        


        public void ActivateCountdown()
        {
            UIEffects.FadeCanvasGroup(_canvasGroup, 1, 0.02f);
            UIEffects.AnimateSliderFill(_fillBar, 1f, _deployDelay);
            HasStarted = true;
        }
        
        
        
        public void Initialize(float delay, Sprite sprite = null)
        {
            Image = GetComponent<Image>();
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
            _fillBar = GetComponentInChildren<Slider>();
            _deployDelay = delay;
            if (sprite != null)
            {
                Image.sprite = sprite;
            }
        }
        
   
        
    }
}


