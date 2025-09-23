using System;
using BackEnd.Base_Classes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace VisualCues
{
    public class CoinPickupEffect : SingletonMonoBehaviour<CoinPickupEffect>
    {
        [Header("References")] public GameObject Coin3DPrefab;
        public GameObject CoinUIPrefab;
        public RectTransform MoneyIcon;


        [Header("Animation Settings")] public int CoinCount = 3;
        public float ShakeDuration = 0.3f;
        public float FloatDuration = 0.6f;


        private Canvas _canvas;
        private RectTransform _canvasRect;


        protected override void Awake()
        {
            base.Awake();
            _canvas = GetComponent<Canvas>();
            _canvasRect = _canvas.GetComponent<RectTransform>();

        }

        public void SpawnCoins(Vector3 worldPosition, Action onComplete = null)
        {
            int completedCount = 0;

            for (int i = 0; i < CoinCount; i++)
            {
                GameObject coin3D = Instantiate(Coin3DPrefab, worldPosition, Quaternion.identity);

                coin3D.transform.DOShakePosition(ShakeDuration, strength: Vector3.one * 0.3f).OnComplete(() =>
                {
                    if (Camera.main == null) return;

                    Vector3 screenPos = Camera.main.WorldToScreenPoint(coin3D.transform.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, _canvas.worldCamera,
                        out var uiPos);

                    GameObject coinUI = Instantiate(CoinUIPrefab, _canvas.transform);
                    RectTransform coinRect = coinUI.GetComponent<RectTransform>();
                    coinRect.anchoredPosition = uiPos;

                    float delay = UnityEngine.Random.Range(0f, 0.07f); 

                    coinRect.DOMove(MoneyIcon.position, FloatDuration)
                        .SetEase(Ease.InOutQuad)
                        .SetDelay(delay)
                        .OnComplete(() =>
                        {
                            Destroy(coinUI);
                            completedCount++;
                            if (completedCount >= CoinCount)
                            {
                                onComplete?.Invoke();
                            }
                        });

                    Destroy(coin3D);
                });
            }
        }
    }
}