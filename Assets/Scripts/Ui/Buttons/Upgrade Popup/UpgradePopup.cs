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
        [SerializeField] private Transform _horizontalLayerGroup;

        private GameObject[] _selectedPrefabs;
        private int _currentIndex;
        private CanvasGroup _canvasGroup;
        private bool _isSpawningPaused;
        private bool _droppingDown;
        private Animator _animator;


        public IReadOnlyList<GameObject> CurrentEligiblePrefabs => _selectedPrefabs;
        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
            _animator  = GetComponent<Animator>();
        }

        

        public void ShowPopup()
        {
            UIEffects.FadeCanvasGroup(_raycastBlockerPanel, 0.4f, 0.2f);
            _droppingDown = true;
            GameStates.Instance.PauseGame();
            _raycastBlockerPanel.blocksRaycasts = true;
            BlockRaycasts(false, false);
            _animator.SetBool("DropDown", _droppingDown); 
        }

        //Animation Event
        public void OnDropInAnimationComplete()
        {
            SpawnAllSlots();
        }
        //Animation Event
        public void OnRiseUpAnimationComplete()
        {
            BlockRaycasts(false);
            ClearAllSlots();
            UIEffects.FadeCanvasGroup(_raycastBlockerPanel, 0, 0.2f);
            GameStates.Instance.StartGame();
        }

        public void HidePopup()
        {
            _droppingDown = false;
            _animator.SetBool("DropDown", _droppingDown); 
            
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
                BlockRaycasts(true, false);
                return;
            }

            var prefab = _selectedPrefabs[_currentIndex];
            Instantiate(prefab, _horizontalLayerGroup);
            
            _currentIndex++;
        }

        private void ClearAllSlots()
        {
            foreach (Transform child in _horizontalLayerGroup)
            {
                Destroy(child.gameObject);
            }

            _selectedPrefabs = null;
            _currentIndex = 0;
        }
        
        public void BlockRaycasts(bool state, bool raycastBlocker = true)
        {
            if (raycastBlocker)
            { 
                _raycastBlockerPanel.blocksRaycasts = state;
            }
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

        #endregion
        
        

        
    }
}