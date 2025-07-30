using System;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Economy;
using turrets;

namespace Ui.Buttons.Turret_Button
{
    public partial class TurretButton
    {
        public void DeployTurret()
        {
            ExecuteTurretButtonAction(
                TurretButtonType.DeployTurret,
                VisualFeedbackType.Flash,
                AddTurretToEmptySlotLogic,
                () => PlayerCurrency.Instance.HasEnoughMoney(Cost) && CanDeployTurret());
        }

        public void AddTurretSlot()
        {
            ExecuteTurretButtonAction(
                TurretButtonType.AddSlot,
                VisualFeedbackType.Flash,
                AddSlotLogic,
                () => PlayerCurrency.Instance.HasEnoughMoney(Cost) && CanAddSlot());
        }

        public void SellTurret()
        {
            ExecuteTurretButtonAction(
                TurretButtonType.SellTurret,
                VisualFeedbackType.Flash,
                SellTurretLogic,
                HaveAnyTurrets);
        }

        private bool CanDeployTurret() => _turretSpawnPoints.Exists(p => !p.HasTurret && p.IsUnlocked);
        private bool CanAddSlot() => _turretSpawnPoints.Exists(p => !p.IsUnlocked);
        private bool HaveAnyTurrets() => _turretSpawnPoints.Exists(p => p.HasTurret);


        private void ExecuteTurretButtonAction(
            TurretButtonType type,
            VisualFeedbackType feedback,
            Action<TurretSpawnPoint> logic,
            Func<bool> currencyCheck)
        {
            if (!currencyCheck()) return;   

            var condition = _conditions[type];
            SetVisualFeedback(condition, feedback);

            CleanupCanvasGroup();

            StartCoroutine(MouseRayCaster.Instance.WaitForMouseClick(
                onValidHit: hit =>
                {
                    if (hit.collider.TryGetComponent<TurretSpawnPoint>(out var slot) && condition(slot))
                    {
                        ConfirmSlotAndInvoke(slot, logic);
                    }
                    ResetVisualFeedBack();
                },
                onMissedClick: ResetVisualFeedBack)
                );
        }


    }
}