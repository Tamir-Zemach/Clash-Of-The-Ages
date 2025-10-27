using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using Configuration;
using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UpgradePopup : InGameObjectSingleton<UpgradePopup>
    {
        public event Action OnSlotsSpawned;
        public event Action OnGettingEligibleList;
        
        [SerializeField] private float _spawnDelay = 0.5f;
        [SerializeField] private CanvasGroup _raycastBlockerPanel;

        private GameObject[] _selectedPrefabs;
        private int _currentIndex;
        private CanvasGroup _canvasGroup;
        private bool _isSpawningPaused;
 

        public IReadOnlyList<GameObject> CurrentEligiblePrefabs => _selectedPrefabs;
        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowPopup()
        {
            UIEffects.FadeCanvasGroup(_canvasGroup, 1, 0.3f, onComplete: () =>
            {
                GameStates.Instance.PauseGame();
                SpawnAllSlots();
            });
        }

        public void HidePopup()
        {
            UIEffects.FadeCanvasGroup(_canvasGroup, 0, 0.3f, onComplete: () =>
            {
                BlockRaycasts(false);
                ClearAllSlots();
                GameStates.Instance.StartGame();
            });
        }

        private void SpawnAllSlots()
        {
            var eligiblePrefabs = UpgradePopupConfiguration.Instance.GetEligiblePrefabs();
            if (eligiblePrefabs == null)
            {
                print("eligiblePrefabs is null");
                return;
            }

            _selectedPrefabs = eligiblePrefabs
                .OrderBy(x => Random.value)
                .Take(3)
                .ToArray();
            OnGettingEligibleList?.Invoke();

            _currentIndex = 0;
            
            InvokeRepeating(nameof(SpawnNextSlot), 0, _spawnDelay);
        }
        private void SpawnNextSlot()
        {
            if (_currentIndex >= _selectedPrefabs.Length && _selectedPrefabs != null)
            {
                CancelInvoke(nameof(SpawnNextSlot));
                OnSlotsSpawned?.Invoke();
                BlockRaycasts(true);
                return;
            }

            var prefab = _selectedPrefabs[_currentIndex];
            Instantiate(prefab, transform);
            
            _currentIndex++;
        }

        private void ClearAllSlots()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            _selectedPrefabs = null;
            _currentIndex = 0;
        }
        
        public void BlockRaycasts(bool state)
        {
            _raycastBlockerPanel.blocksRaycasts = state;
            _canvasGroup.interactable = state;
            _canvasGroup.blocksRaycasts = state;
        }

        #region Unity Lifecycle
        protected override void HandlePause()
        {
            if (IsInvoking(nameof(SpawnNextSlot)))
            {
                CancelInvoke(nameof(SpawnNextSlot));
                _isSpawningPaused = true;
            }
        }

        protected override void HandleResume()
        {
            if (_isSpawningPaused)
            {
                _isSpawningPaused = false;
                InvokeRepeating(nameof(SpawnNextSlot), 0, _spawnDelay);
            }
        }

        protected override void HandleGameEnd()
        {
            CancelInvoke(nameof(SpawnNextSlot));
            _isSpawningPaused = false;
            HidePopup();
            ClearAllSlots();
        }

        protected override void HandleGameReset()
        {
            CancelInvoke(nameof(SpawnNextSlot));
            _isSpawningPaused = false;
            HidePopup();
            ClearAllSlots();
        }
        

        #endregion

        /*
        [SerializeField] private Animator _animator;

        public void ShowPopup()
        {
            GameStates.Instance.PauseGame();
            _animator.Play("DropIn"); // your animation clip state
        }

        public void OnDropInAnimationComplete()
        {
            // This can be called via an Animation Event at the end of the clip
            SpawnAllSlots();
        }
        */
        

        
    }
}