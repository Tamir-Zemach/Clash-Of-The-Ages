using System.Collections;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace turrets
{
    public class TurretSpawnPoint : MonoBehaviour
    {
        [FormerlySerializedAs("highlightGfx")] [SerializeField] private GameObject _highlightGfx;

        [field: SerializeField] public bool IsFriendly { get; private set; }

        public bool IsUnlocked {  get; set; }

        public bool HasTurret { get; set; }
        
        public TurretType TurretType { get; set; }


        private ManagedCoroutine _flashRoutine;
        
        public void ShowHighlight(bool show)
        {
            if (_highlightGfx != null)
                _highlightGfx.SetActive(show);
        }

        public void StartFlashing(float timeBetweenFlashes)
        {
            if (_flashRoutine == null)
            {
                _flashRoutine = CoroutineManager.Instance.StartManagedCoroutine(FlashLoop(timeBetweenFlashes));
            }
        }

        public void StopFlashing()
        {
            if (_flashRoutine != null)
            {
                _flashRoutine.Stop();
                _highlightGfx.SetActive(false);
                _flashRoutine = null;
            }
        }

        private IEnumerator FlashLoop(float timeBetweenFlashes)
        {
            while (true)
            {
                _highlightGfx.SetActive(true);
                yield return new WaitForSeconds(timeBetweenFlashes);
                _highlightGfx.SetActive(false);
                yield return new WaitForSeconds(timeBetweenFlashes);
            }
        }
    }
}
