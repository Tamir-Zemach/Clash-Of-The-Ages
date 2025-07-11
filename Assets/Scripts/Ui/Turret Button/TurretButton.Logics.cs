using Assets.Scripts.Enems;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ui.TurretButton
{


    public partial class TurretButton 
    {
        /// <summary>
        /// Coroutine that listens for a left-click on a valid turret spawn point.
        /// If a valid slot is clicked, invokes the corresponding logic and clears visual feedback.
        /// If an invalid click occurs, cancels the action and also clears visuals.
        /// </summary>
        /// <param name="isValidSlot">A condition to validate the clicked TurretSpawnPoint.</param>
        /// <param name="logicToExecute">The logic to execute if a valid slot is selected.</param>
        private IEnumerator WaitForSlotClick(Func<TurretSpawnPoint, bool> isValidSlot,
            Action<TurretSpawnPoint> logicToExecute)
        {

            _isWaitingForClick = true;
            SetVisualFeedback(point => point.IsUnlocked, VisualFeedbackType.Highlight);
            while (_isWaitingForClick)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var hit = MouseRayCaster.Instance.GetHit();

                    // Check if we hit a collider and it's a TurretSpawnPoint
                    if (hit.HasValue && hit.Value.collider.TryGetComponent<TurretSpawnPoint>(out var slot))
                    {
                        // If the slot meets the criteria, confirm and invoke logic
                        if (isValidSlot(slot))
                        {
                            ConfirmSlotAndInvoke(slot, logicToExecute);
                            yield break; // Exit coroutine
                        }
                    }

                    // Click was invalid: stop waiting and remove visual feedback
                    _isWaitingForClick = false;
                    ResetVisualFeedBack();
                    yield break; // Exit coroutine
                }
                // Wait for next frame before continuing loop
                yield return null;
            }
        }

        private void ConfirmSlotAndInvoke(TurretSpawnPoint slot, Action<TurretSpawnPoint> logicToExecute)
        {
            _isWaitingForClick = false;

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