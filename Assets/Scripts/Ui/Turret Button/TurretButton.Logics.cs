using Assets.Scripts.BackEnd.Enems;
using System;
using BackEnd.Economy;
using turrets;
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
            if (!turret) return;
            Destroy(turret.gameObject);
            
        }

        private void AddTurretToEmptySlotLogic(TurretSpawnPoint slot)
        {
            PlayerCurrency.Instance.SubtractMoney(_cost);
            slot.HasTurret = true;

            var turret = Instantiate(_turret.Prefab, slot.transform.position, slot.transform.rotation);
            turret.transform.parent = slot.transform;
            var behaviour = turret.GetComponent<TurretBaseBehavior>();

            if (behaviour)
            {
                behaviour.Initialize(_turret);
            }
            else
            {
                Debug.LogWarning("TurretBaseBehaviour not found on spawned enemy prefab.");
            }


        }


    }
}