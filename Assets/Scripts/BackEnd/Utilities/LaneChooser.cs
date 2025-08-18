using System;
using Managers;
using Ui.Buttons.Deploy_Button;

namespace BackEnd.Utilities
{
    public static class LaneChooser
    {
        public static void ChooseLane(Action<Lane> onLaneChosen, Action onCancel)
        {
            MouseRayCaster.Instance.StartClickRoutine(
                onValidHit: hit =>
                {
                    var lane = hit.collider.GetComponentInParent<Lane>();
                    if (lane != null && !lane.IsDestroyed)
                    {
                        onLaneChosen?.Invoke(lane);
                    }
                    else
                    {
                        onCancel?.Invoke();
                    }
                },
                onMissedClick: () =>
                {
                    onCancel?.Invoke();
                });
        }
    }
}