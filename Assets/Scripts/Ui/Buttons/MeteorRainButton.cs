using BackEnd.Enums;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using Bases;
using DG.Tweening;
using Managers;
using Special_Attacks;
using Ui.Buttons.Deploy_Button;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui.Buttons
{
    public class MeteorRainButton : ButtonWithCost, IImageSwitchable<SpecialAttackType>
    {
        
        public UnityEvent OnTimerStarted;
        public UnityEvent OnTimerStoped;

        public SpecialAttackType Type { get; } = SpecialAttackType.DestroyPath;

        [SerializeField] private float _specialAttackTimer;
        
        private Slider _cooldownSlider;

        private SpecialAttackData _meteorRain;

        private Image _image;
        
        private Tween _cooldownTween;
        
        private Lane _defaultLane;

        private bool _meteorRainAccruing;


        private void Start()
        {
            GetData();
        }
        

        private void GetData()
        {
            _meteorRain = GameDataRepository.Instance.FriendlySpecialAttacks.GetData(Type);
            _cooldownSlider = GetComponentInChildren<Slider>();
            _image = GetComponent<Image>();
            ResetTimer();
            if (EnemyBasesManager.Instance.MultipleBases()) return;
            
            _defaultLane = FindAnyObjectByType<Lane>();
        }

        private void MeteorRainInEnded()
        {
            _meteorRainAccruing = false;
        }

        private void MeteorRainInProgress()
        {
            _meteorRainAccruing = true;
        }
        
        public void PerformSpecialAttack()
        {
            if (!CanPerformAttack()) return;
            if (EnemyBasesManager.Instance.MultipleBases())
            {
                LaneChooser.ChooseLane(
                    onLaneChosen: lane =>
                    {
                        if (_meteorRainAccruing) return;
                        ExecuteSpecialAttackOnLane(lane);
                    },
                    onCancel: () =>
                    {
                        Debug.Log("Lane selection canceled or invalid.");
                    });
            }
            else
            {
                ExecuteSpecialAttackOnLane();
            }

        }

        private void ExecuteSpecialAttackOnLane(Lane lane = null)
        {
            PlayerCurrency.Instance.SubtractMoney(Cost);
            if (lane != null)
            {
                lane.MeteorRainSpawnPosition.ApplySpecialAttack();
            }
            else
            {
                _defaultLane.MeteorRainSpawnPosition.ApplySpecialAttack();
            }
            SpawnSpecialAttack(lane);
            ResetTimer();
        }

        private void SpawnSpecialAttack(Lane lane = null)
        {
            var selectedLane = lane != null ? lane : _defaultLane;
            
            var meteorRainTransform = selectedLane.MeteorRainSpawnPosition.gameObject.transform;
            
            var laneTransform = selectedLane.gameObject.transform;
            
            var specialAttack = Instantiate(_meteorRain.Prefab, meteorRainTransform.position,  meteorRainTransform.localRotation);
            
            var behaviour = specialAttack.GetComponent<SpecialAttackBaseBehavior>();

            if (behaviour != null)
            {
                behaviour.Initialize(_meteorRain, selectedLane.MeteorRainSpawnPosition);
            }
            else
            {
                Debug.LogWarning("SpecialAttackBaseBehaviour not found on spawned enemy prefab.");
            }

        }

        private bool CanPerformAttack()
        {
            return PlayerCurrency.Instance.HasEnoughMoney(Cost)
                   && !_meteorRainAccruing
                   && _cooldownSlider.value <= 0;
        }


 
        
        private void ResetTimer()
        {
            _cooldownSlider.value = 1;
            OnTimerStarted?.Invoke();

            _cooldownTween = UIEffects.AnimateSliderFill(_cooldownSlider, 0, _specialAttackTimer, () =>
            {
                UIEffects.ApplyGraphicFeedback(
                    graphic: _image,
                    shakeGraphic: true, shakeDuration: 0.3f,
                    changeScale: true, scaleMultiplier: 1.2f, scaleChangeDuration: 0.2f,
                    onComplete: () => { OnTimerStoped?.Invoke(); }
                );
            });
        }

        #region GameLifecycle
            //TODO: make a helper/use the base class "GameStatesSubscriber
            private void OnEnable()
            {
                GameStates.Instance.OnGamePaused += PauseCooldown;
                GameStates.Instance.OnGameResumed += ResumeCooldown;
                GameStates.Instance.OnGameEnded += CancelCooldown;
                MeteorRainSpawnPos.OnMeteorRainAccruing += MeteorRainInProgress;
                MeteorRainSpawnPos.OnMeteorRainEnding += MeteorRainInEnded;
            }

            private void OnDisable()
            {
                GameStates.Instance.OnGamePaused -= PauseCooldown;
                GameStates.Instance.OnGameResumed -= ResumeCooldown;
                GameStates.Instance.OnGameEnded -= CancelCooldown;
                MeteorRainSpawnPos.OnMeteorRainAccruing -= MeteorRainInProgress;
                MeteorRainSpawnPos.OnMeteorRainEnding -= MeteorRainInEnded;
            }

            private void PauseCooldown()
            {
                _cooldownTween?.Pause();
            }

            private void ResumeCooldown()
            {
                _cooldownTween?.Play();
            }

            private void CancelCooldown()
            {
                _cooldownTween?.Kill();
                _cooldownTween = null;
                _cooldownSlider.value = 0f;
            }

        #endregion
    
    }
}
