using System;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UpgradePopup : PersistentMonoBehaviour<UpgradePopup>
    {
        [SerializeField] private GameObject[] _upgradeSlotsPrefabs;
        [SerializeField] private float _spawnDelay = 0.5f;
        [SerializeField] private CanvasGroup _raycastBlockerPanel;

        private GameObject[] _selectedPrefabs;
        private int _currentIndex;
        private CanvasGroup _canvasGroup;
        private Action _onSlotsSpawned;

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
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;

                    _raycastBlockerPanel.blocksRaycasts = true;
                });
            });
        }

        public void HidePopup()
        {
            UIEffects.FadeCanvasGroup(_canvasGroup, 0, 0.3f, onComplete: () =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                ClearAllSlots();
                _raycastBlockerPanel.blocksRaycasts = false;
                GameStates.Instance.StartGame();
            });
        }

        private void SpawnAllSlots(Action onComplete = null)
        {
            if (_upgradeSlotsPrefabs == null || _upgradeSlotsPrefabs.Length < 3)
                return;

            _selectedPrefabs = _upgradeSlotsPrefabs
                .OrderBy(x => Random.value)
                .Take(3)
                .ToArray();

            _currentIndex = 0;
            _onSlotsSpawned = onComplete;
            InvokeRepeating(nameof(SpawnNextSlot), 0, _spawnDelay);
        }

        private void SpawnNextSlot()
        {
            if (_currentIndex >= _selectedPrefabs.Length && _selectedPrefabs != null)
            {
                CancelInvoke(nameof(SpawnNextSlot));
                _onSlotsSpawned?.Invoke(); 
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
    }
}