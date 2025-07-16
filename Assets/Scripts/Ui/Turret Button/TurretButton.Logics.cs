using Assets.Scripts.Enems;
using System;
using UnityEngine;

namespace Assets.Scripts.Ui.TurretButton
{


    public partial class TurretButton 
    {

        private void ConfirmSlotAndInvoke(TurretSpawnPoint slot, Action<TurretSpawnPoint> logicToExecute)
        { 
            ResetVisualFeedBack();

            // Perform the action tied to this interaction
            logicToExecute?.Invoke(slot);
        }

        private void ResetVisualFeedBack()
        {
            // Stop flashing on all spawn points
            SetVisualFeedback(_ => true, VisualFeedbackType.StopFlash);

            // Keep unlocked slots highlighted
            SetVisualFeedback(point => point.IsUnlocked, VisualFeedbackType.Highlight);

            // Turn off visibility for all locked slots
            SetVisualFeedback(point => !point.IsUnlocked, VisualFeedbackType.Off);

            ShowCanvas();

            ////if there is an overlay - fade it away
            //CleanupOverlay();
        }



        //Actual logic to aplly when all the conditions are matching 
        private void AddSlotLogic(TurretSpawnPoint slot)
        {
            PlayerCurrency.Instance.SubtractMoney(_cost);
            slot.IsUnlocked = true;

            SetVisualFeedback(point => point.IsUnlocked, VisualFeedbackType.Highlight);

            // onSlotUnlocked?.Invoke();
        }

        private void SellTurretLogic(TurretSpawnPoint slot)
        {
            PlayerCurrency.Instance.AddMoney(_moneyToGiveBack);
            slot.HasTurret = false;

            var turret = slot.GetComponentInChildren<TurretBaseBehavior>();
            if (turret != null)
            {
                Destroy(turret.gameObject);
            }


            // onTurretSelled?.Invoke();
        }

        private void AddTurretToEmptySlotLogic(TurretSpawnPoint slot)
        {
            PlayerCurrency.Instance.SubtractMoney(_cost);
            slot.HasTurret = true;

            GameObject turret = Instantiate(_turret.Prefab, slot.transform.position, slot.transform.rotation);

            turret.transform.parent = slot.transform;

            // onTurretPlaced?.Invoke();
        }


    }
}