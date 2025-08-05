using System;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UpgradePopup : PersistentMonoBehaviour<UpgradePopup>
    {
        [SerializeField] private float _spawnDelay = 0.5f;
        [SerializeField] private CanvasGroup _raycastBlockerPanel;

        private GameObject[] _selectedPrefabs;
        private int _currentIndex;
        private CanvasGroup _canvasGroup;
        public Action OnSlotsSpawned;

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
                SpawnAllSlots(() =>
                {
                    BlockRaycasts(true);
                });
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

        private void SpawnAllSlots(Action onComplete = null)
        {
            var eligiblePrefabs = UpgradePopupConfiguration.Instance.GetEligiblePrefabs();

            if (eligiblePrefabs == null || eligiblePrefabs.Count < 3)
                return;

            _selectedPrefabs = eligiblePrefabs
                .OrderBy(x => Random.value)
                .Take(3)
                .ToArray();

            _currentIndex = 0;
            OnSlotsSpawned = onComplete;
            InvokeRepeating(nameof(SpawnNextSlot), 0, _spawnDelay);
        }
        private void SpawnNextSlot()
        {
            if (_currentIndex >= _selectedPrefabs.Length && _selectedPrefabs != null)
            {
                CancelInvoke(nameof(SpawnNextSlot));
                OnSlotsSpawned?.Invoke(); 
                return;
            }

            Instantiate(_selectedPrefabs[_currentIndex], transform);
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
        
    }
}