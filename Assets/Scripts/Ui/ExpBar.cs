using BackEnd.Economy;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class ExpBar: MonoBehaviour
    {
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>(); 
            PlayerExp.OnExpChanged += SetExp;
            PlayerExp.OnExpToLevelUpChanged += SetExpToLevelUp;
            
        }

        private void Start()
        {
            SetExpToLevelUp();
        }

        private void SetExpToLevelUp()
        {
            _slider.maxValue = PlayerExp.Instance.ExpToLevelUp;
        }

        private void SetExp()
        {
            UIEffects.AnimateSliderFill(_slider, PlayerExp.Instance.CurrentExp, 0.5f);
        }
    }
}