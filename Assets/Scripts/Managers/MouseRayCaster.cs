using System;
using System.Collections;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using Managers.Loaders;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class MouseRayCaster : SceneAwareMonoBehaviour<MouseRayCaster>
    {
        [SerializeField] private LayerMask raycastLayers;
        [SerializeField] private float raycastDistance = 100f;

        private GameObject _currentHover;
        private UnityEngine.Camera _rayCamera;
        private ManagedCoroutine _activeClickRoutine;
        

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _rayCamera = _rayCamera != null ? _rayCamera : UnityEngine.Camera.main;
        }

        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            HandleHover();
        }

        private void HandleHover()
        {
            GameObject hitObject = GetHitObject();
            if (hitObject != _currentHover)
            {
                _currentHover = hitObject;
            }
        }

        public GameObject GetHitObject() => GetHit()?.collider.gameObject;

        public RaycastHit? GetHit()
        {
            if (_rayCamera == null) return null;
            Ray ray = _rayCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

            return Physics.Raycast(ray, out RaycastHit hit, raycastDistance, raycastLayers) ? hit : null;
        }

        public void StartClickRoutine(Action<RaycastHit> onValidHit = null, Action onMissedClick = null)
        {
            // Cancel any existing managed coroutine
            if (_activeClickRoutine != null)
            {
                _activeClickRoutine.Stop();
                _activeClickRoutine = null;
            }

            _activeClickRoutine = new ManagedCoroutine(WaitForMouseClickInternal(onValidHit, onMissedClick), this);
            _activeClickRoutine.Start();
        }
        private IEnumerator WaitForMouseClickInternal(Action<RaycastHit> onValidHit = null, Action onMissedClick = null)
        {
            yield return new WaitForSeconds(0.1f); // buffer before click

            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var hit = GetHit();

                    if (hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                    {
                        onValidHit?.Invoke(hit.Value);
                        break;
                    }

                    if (!hit.HasValue && !EventSystem.current.IsPointerOverGameObject())
                    {
                        onMissedClick?.Invoke();
                        break;
                    }
                }

                yield return null;
            }
        }
        
        public void CancelClickRoutine()
        {
            if (_activeClickRoutine != null)
            {
                _activeClickRoutine.Stop();
                _activeClickRoutine = null;
            }
        }
    }
}