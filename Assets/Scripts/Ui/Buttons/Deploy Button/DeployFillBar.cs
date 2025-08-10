using System;
using BackEnd.Enums;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Utilities;
using DG.Tweening;
using Managers.Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Buttons.Deploy_Button
{
    [RequireComponent(typeof(UnitDeployButton))]
    public class DeployFillBar : MonoBehaviour
    {
        [SerializeField] private Slider _fillBar;
        
        private UnitDeployButton _unitDeployButton;
        private UnitType  _unitType;
        private UnitData  _unitData;
        private CanvasGroup _canvasGroup;
        private Tween _countdownTween;
        private bool _hasStarted;

        private void Awake()
        {
            DeployManager.OnUnitReadyToDeploy += ActivateCountdown;
            _unitDeployButton = GetComponent<UnitDeployButton>();
            _unitType = _unitDeployButton.Type;
            _unitData = GameDataRepository.Instance.FriendlyUnits.GetData(_unitType);
            _canvasGroup = _fillBar.gameObject.GetComponent<CanvasGroup>();
            
        }

        private void OnDestroy()
        {
            DeployManager.OnUnitReadyToDeploy -= ActivateCountdown;
        }

        private void ActivateCountdown(UnitData unitData)
        {
            if (unitData.Type == _unitType)
            {
                _countdownTween?.Kill();
                _fillBar.value = 1;
                UIEffects.FadeCanvasGroup(_canvasGroup, 1, 0.02f);
                _countdownTween = UIEffects.AnimateSliderFill(_fillBar, 0, _unitData.DeployDelayTime, 
                onComplete: () => { UIEffects.FadeCanvasGroup(_canvasGroup, 0, 0.02f); });
            }
        }
        
        
        
        
        
        
        
    }
}
