using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Enums;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using turrets;
using UnityEngine;

namespace Ui.Buttons.Turret_Button
{


    public partial class TurretButton 
    {
        private void ConfirmSlotAndInvoke(TurretSpawnPoint slot, Action<TurretSpawnPoint> logicToExecute)
        { 
            ResetVisualFeedBack();

            // Perform the action tied to this interaction
            logicToExecute?.Invoke(slot);
        }
        

        //Actual logic to aplly when all the conditions are matching 
        private void AddSlotLogic(TurretSpawnPoint slot)
        {
            PlayerCurrency.Instance.SubtractMoney(Cost);
            slot.IsUnlocked = true;

            SetVisualFeedback(point => point.IsUnlocked, VisualFeedbackType.Highlight);

            // onSlotUnlocked?.Invoke();
        }

        private void SellTurretLogic(TurretSpawnPoint slot)
        {
            var refundAmount = GetOtherTurretCost(slot.TurretType) / 2;

            if (refundAmount <= 0)
            {
                Debug.LogWarning($"No refund amount for turret type {slot.TurretType}");
                return;
            }

            PlayerCurrency.Instance.AddMoney(refundAmount);
            slot.HasTurret = false;

            var turret = slot.GetComponentInChildren<TurretBaseBehavior>();
            if (!turret) return;
            Destroy(turret.gameObject);
        }

        private void AddTurretToEmptySlotLogic(TurretSpawnPoint slot)
        {
            PlayerCurrency.Instance.SubtractMoney(Cost);
            slot.HasTurret = true;
            slot.TurretType = _turretType;

            var turret = Instantiate(_turret.Prefab, slot.transform.position, slot.transform.rotation);
            turret.transform.parent = slot.transform;
            var behaviour = turret.GetComponent<TurretBaseBehavior>();

            if (behaviour)
            {
                behaviour.Initialize(_turret, _playerBase.transform);
            }
            else
            {
                Debug.LogWarning("TurretBaseBehaviour not found on spawned enemy prefab.");
            }

        }
        

        private int GetOtherTurretCost(TurretType typeToFind)
        {
            var otherButtons = UIObjectFinder.GetButtons<TurretButton, TurretType>();

            foreach (var button in otherButtons)
            {
                if (button != null && button.Type == typeToFind)
                {
                    return button.Cost;
                }
            }

            Debug.LogWarning($"No turret button found for type {typeToFind}");
            return 0;
        }


    }
}