using DG.Tweening;
using UnityEngine;

namespace BackEnd.Base_Classes
{
    public class PopupSlot : MonoBehaviour
    {
        //TODO: Fix starting animations
        private void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 originalPos = rectTransform.anchoredPosition;

                rectTransform.DOAnchorPos(originalPos + new Vector2(0, 40), 0.2f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        rectTransform.DOAnchorPos(originalPos, 0.2f).SetEase(Ease.InQuad);
                    });
            }
        }
    }
}