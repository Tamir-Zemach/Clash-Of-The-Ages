using System;
using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using DG.Tweening;
using Managers;
using Managers.Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Buttons.Deploy_Button
{
    public class UnitQueuePanelHandler : InGameObject
    {
        [SerializeField] private UnitQueueItemUI[] _queueItems; // Prefabs only

        private readonly List<UnitQueueItemUI> _activeQueueItems = new();
        private Dictionary<UnitType, Sprite> _unitSpriteLookup = new();

        private int _globalIdCounter = 0;
        private int _activeCountdownId;

        private void Awake()
        {
            DeployManager.OnUnitQueued += CreateQueueSlot;
            DeployManager.OnUnitReadyToDeploy += ActivateCountdown;
            DeployManager.OnUnitDeployed += HandleUnitDeploymentEnd;
            UiAgeUpgrade.Instance.OnUiRefreshDeployUnits += UpdateSprites;
        }
        private void OnDestroy()
        {
            DeployManager.OnUnitQueued -= CreateQueueSlot;
            DeployManager.OnUnitReadyToDeploy -= ActivateCountdown;
            DeployManager.OnUnitDeployed -= HandleUnitDeploymentEnd;
            UiAgeUpgrade.Instance.OnUiRefreshDeployUnits -= UpdateSprites;
        }

        private void UpdateSprites(List<SpriteEntries.SpriteEntry<UnitType>> spriteMap)
        {
            _unitSpriteLookup.Clear();
            foreach (var entry in spriteMap)
            {
                _unitSpriteLookup[entry.GetKey()] = entry.GetSprite();
            }
        }

        private void CreateQueueSlot(UnitData unitData, int position)
        {
            var itemPrefab = GetRelevantItem(unitData.Type);
            if (itemPrefab == null) return;

            var newItemGo = Instantiate(itemPrefab.gameObject, transform);
            var queueItem = newItemGo.GetComponent<UnitQueueItemUI>();
            if (queueItem != null)
            {
                queueItem.ID = ++_globalIdCounter;
                var sprite = _unitSpriteLookup.TryGetValue(unitData.Type, out var foundSprite) ? foundSprite : null;
                queueItem.Initialize(unitData.DeployDelayTime, sprite);
                _activeQueueItems.Add(queueItem);
            }
        }

        private void ActivateCountdown(UnitData unitData)
        {
            foreach (var item in _activeQueueItems)
            {
                if (item == null || item.Type != unitData.Type || item.HasStarted) continue;

                _activeCountdownId = item.ID;
                item.ActivateCountdown();
                break;
            }
        }

        private void HandleUnitDeploymentEnd()
        {
            for (int i = _activeQueueItems.Count - 1; i >= 0; i--)
            {
                var item = _activeQueueItems[i];
                if (item != null && item.ID == _activeCountdownId)
                {
                    UIEffects.ShrinkAndDestroy(item.transform, 0.7f, 0.2f, () => Destroy(item.gameObject));
                    _activeQueueItems.RemoveAt(i);
                    break;
                }
            }
        }

        private UnitQueueItemUI GetRelevantItem(UnitType unitType)
        {
            foreach (var item in _queueItems)
            {
                if (item.Type == unitType)
                {
                    return item;
                }
            }
            return null;
        }

        #region GameLifecycle

        protected override void HandlePause()
        {
            foreach (var item in _activeQueueItems)
            {
                item?.PauseCountdown();
            }
        }

        protected override void HandleResume()
        {
            foreach (var item in _activeQueueItems)
            {
                item?.ResumeCountdown();
            }
        }

        protected override void HandleGameEnd()
        {
            // Optional: Clear queue or disable UI
        }

        protected override void HandleGameReset()
        {
            // Optional: Reset queue state
        }

        #endregion

    }
}


//TODO: Change this SHITSHOW - the update sprite logic