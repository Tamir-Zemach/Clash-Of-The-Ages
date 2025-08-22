using System.Collections;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using UnityEngine;

namespace VisualCues
{
    public class HighlightGfx : MonoBehaviour
    {
        [SerializeField] private GameObject _highlightVisual;
        private ManagedCoroutine _flashRoutine;
        private Vector3 _defaultScale;

        private void Awake()
        {
            if (_highlightVisual != null)
            {
                _defaultScale = _highlightVisual.transform.localScale;
            }
        }

        public void Show(bool show)
        {
            if (_highlightVisual != null)
            {
                _highlightVisual.SetActive(show);
            }
        }

        public void StartFlashing(float interval)
        {
            if (_flashRoutine == null)
            {
                _flashRoutine = CoroutineManager.Instance.StartManagedCoroutine(FlashLoop(interval));
            }

        }

        public void StopFlashing()
        {
            if (_flashRoutine != null)
            {
                _flashRoutine.Stop();
                Show(false);
                _flashRoutine = null;
            }
        }

        public void ShrinkAndHide(
            Vector3 growMultiplier,
            Vector3 shrinkMultiplier,
            float growDuration = 0.15f,
            float shrinkDuration = 0.1f)
        {
            if (_highlightVisual == null) return;

            StopFlashing();
            _highlightVisual.SetActive(true);

            GameObjectEffects.ShrinkWithMultipliers(
                _highlightVisual,
                _defaultScale,
                growMultiplier,
                shrinkMultiplier,
                growDuration,
                shrinkDuration,
                () =>
                {
                    _highlightVisual.SetActive(false);
                    _highlightVisual.transform.localScale = _defaultScale;
                });
        }

        private IEnumerator FlashLoop(float interval)
        {
            while (true)
            {
                Show(true);
                yield return new WaitForSeconds(interval);
                Show(false);
                yield return new WaitForSeconds(interval);
            }
        }
    }
}