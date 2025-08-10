using BackEnd.InterFaces;
using Ui;
using Ui.Text_Display;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BackEnd.Base_Classes
{
    public class ButtonWithCost : MonoBehaviour, ICostable,  IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("Cost for triggering this action.")]
        [SerializeField] private int _cost;
        public int Cost { get => _cost; set => _cost = value; }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            HoverCostDisplay.Instance.ShowTooltip(eventData, Cost, label: "$", color: Color.black);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            HoverCostDisplay.Instance.HideTooltip(eventData);
        }

#if UNITY_EDITOR
        public static class ButtonWithCostFields
        {
            public const string Cost = nameof(_cost);
        }
#endif

    }
}