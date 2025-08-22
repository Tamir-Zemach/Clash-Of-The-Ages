using System;
using Managers;
using Ui.Buttons.Deploy_Button;
using UnityEngine;

namespace BackEnd.Utilities
{
    public static class LaneChooser
    {
        public static void ChooseLane(Action<Lane> onLaneChosen, Action onCancel)
        {
            LaneManager.Instance.StartFlashingAllLanes();
            MouseRayCaster.Instance.StartClickRoutine(
                onValidHit: hit =>
                {
                    
                    var lane = hit.collider.GetComponentInParent<Lane>();
                    if (lane != null && !lane.IsDestroyed)
                    {
                        onLaneChosen?.Invoke(lane);
                        LaneManager.Instance.StopFlashingAllLanes();
                        lane.ShrinkAndHide();
                    }
                    else
                    {
                        LaneManager.Instance.StopFlashingAllLanes();
                        onCancel?.Invoke();
                    }

                },
                onMissedClick: () =>
                {
                    onCancel?.Invoke();
                    LaneManager.Instance.StopFlashingAllLanes();
                });
        }
    }
}