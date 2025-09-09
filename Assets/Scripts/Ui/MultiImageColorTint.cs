using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    [RequireComponent(typeof(Selectable))]
    public class MultiImageColorTint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private List<Image> _imagesToExclude = new List<Image>();

        private Selectable _selectable;
        private readonly List<Image> _targetImages = new List<Image>();

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _targetImages.AddRange(GetComponentsInChildren<Image>(true));

            // Remove excluded images from the target list
            foreach (var excluded in _imagesToExclude)
            {
                if (excluded != null && _targetImages.Contains(excluded))
                    _targetImages.Remove(excluded);
            }
        }

        private void ApplyColor(Color color)
        {
            foreach (var img in _targetImages)
            {
                if (img != null)
                    img.color = color;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;
            ApplyColor(_selectable.colors.highlightedColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;
            ApplyColor(_selectable.colors.normalColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;
            ApplyColor(_selectable.colors.pressedColor);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;
            ApplyColor(_selectable.colors.highlightedColor);
        }

        void OnEnable()
        {
            ApplyColor(_selectable.interactable ? _selectable.colors.normalColor : _selectable.colors.disabledColor);
        }

        void OnDisable()
        {
            ApplyColor(_selectable.colors.disabledColor);
        }
    }
}