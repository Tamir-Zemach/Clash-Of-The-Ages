using BackEnd.Enums;
using UnityEngine;
using VisualCues;

namespace turrets
{
    [RequireComponent(typeof(HighlightGfx))]
    public class TurretSpawnPoint : MonoBehaviour
    {
        [field: SerializeField] public bool IsFriendly { get; private set; }
        public bool IsUnlocked { get; set; }
        public bool HasTurret { get; set; }
        public TurretType TurretType { get; set; }

        private HighlightGfx _highlightGfx;

        private void Awake()
        {
            _highlightGfx = GetComponent<HighlightGfx>();
            if (_highlightGfx == null)
            {
                Debug.LogWarning("HighlightGfx component missing on TurretSpawnPoint.");
            }
        }

        public void ShowHighlight(bool show)
        {
            _highlightGfx?.Show(show);
        }

        public void StartFlashing(float interval)
        {
            _highlightGfx?.StartFlashing(interval);
        }

        public void StopFlashing()
        {
            _highlightGfx?.StopFlashing();
        }
    }
}