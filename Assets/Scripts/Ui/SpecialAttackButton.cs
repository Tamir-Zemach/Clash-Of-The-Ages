using System;
using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts.InterFaces;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Economy;
using Managers;
using Special_Attacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui
{
    public class SpecialAttackButton : MonoBehaviour, IImgeSwichable<SpecialAttackType>
    {
        
        public UnityEvent OnTimerStarted;
        public UnityEvent OnTimerStoped;
        [field: SerializeField] public SpecialAttackType Type {  get; private set; }

        [SerializeField] private int _cost;
        
        [SerializeField] private float _specialAttackTimer;

        private SpecialAttackSpawnPos _specialAttackSpawnPos;

        private SpecialAttackData _specialAttack;

        private Image _image;
        private float _timer;



        private void Start()
        {
            GetData();
        }

        private void Update()
        {
            IsTimerFinished();
        }

        private void GetData()
        {
            _specialAttack = GameDataRepository.Instance.FriendlySpecialAttacks.GetData(Type);
            _image = GetComponent<Image>();
            LevelLoader.Instance.OnSceneChanged += GetSpawnPos;
            GameManager.Instance.OnAgeUpgrade += UpdateSprite;
            GetSpawnPos();
            ResetTimer();
        }

        private void GetSpawnPos()
        {
            _specialAttackSpawnPos = FindAnyObjectByType<SpecialAttackSpawnPos>(); 
        }

        private void UpdateSprite(List<LevelUpDataBase> upgradeDataList)
        {
            foreach (var data in upgradeDataList)
            {
                if (data is SpritesLevelUpData levelUpData)
                {
                    _image.sprite = levelUpData.GetSpriteFromList(Type, levelUpData.SpecialAttackSpriteMap);
                }
            }
        }

        public void PerformSpecialAttack()
        {
            if (!CanPreformAttack()) return;
            PlayerCurrency.Instance.SubtractMoney(_cost);
            _specialAttackSpawnPos.IsSpecialAttackAccruing = true;
            SpawnSpecialAttack();
            ResetTimer();
        }

        private void SpawnSpecialAttack()
        {
            var specialAttack = Instantiate(_specialAttack.Prefab, _specialAttackSpawnPos.transform.position, _specialAttackSpawnPos.transform.rotation);
            var behaviour = specialAttack.GetComponent<SpecialAttackBaseBehavior>();

            if (behaviour != null)
            {
                behaviour.Initialize(_specialAttack, _specialAttackSpawnPos);
            }
            else
            {
                Debug.LogWarning("SpecialAttackBaseBehaviour not found on spawned enemy prefab.");
            }

        }

        private bool CanPreformAttack()
        {
            return PlayerCurrency.Instance.HasEnoughMoney(_cost) && !_specialAttackSpawnPos.IsSpecialAttackAccruing && IsTimerFinished();
        }

        private bool IsTimerFinished()
        {
            _timer += Time.deltaTime;
            if (_timer > _specialAttackTimer)
            {
                OnTimerStoped?.Invoke();
                return true;
            }
            else
            {
                return false;  
            }
        }
        
        private void ResetTimer()
        {
            _timer = 0;
            OnTimerStarted?.Invoke();
        }
    
    
    }
}
