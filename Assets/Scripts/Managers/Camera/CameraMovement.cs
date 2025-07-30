using System;
using BackEnd.Base_Classes;
using Unity.Cinemachine;
using UnityEngine;

namespace Managers.Camera
{
    public class CameraMovement : SceneAwareMonoBehaviour<CameraMovement>
    {
        private CinemachineSplineDolly _dollyCart;
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _edgeThreshold = 50f;
        [SerializeField] private float _pathLength = 1f; // Set to 1 for normalized mode; adjust if using distance-based position
        public static event Action OnCameraMoved;
        private Vector3 _lastCameraPosition;
        private UnityEngine.Camera _camera;
    

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _camera = UnityEngine.Camera.main;
            _dollyCart = FindFirstObjectByType<CinemachineSplineDolly>();
        }

    
        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            if (_camera)
            {
                var currentPosition = _camera.transform.position;
                if (currentPosition != _lastCameraPosition)
                {
                    _lastCameraPosition = currentPosition;
                    OnCameraMoved?.Invoke();
                }
            }
            EdgeScrollWithMouse();
            EdgeScrollWithKeyboard();
        }

        private void EdgeScrollWithMouse()
        {
            if (!_dollyCart) return;
            var mouseX = Input.mousePosition.x;
            var newPosition = _dollyCart.CameraPosition;

            if (mouseX < _edgeThreshold)
            {
                newPosition -= _moveSpeed * Time.deltaTime;
            }
            else if (mouseX > Screen.width - _edgeThreshold)
            {
                newPosition += _moveSpeed * Time.deltaTime;
            }

            _dollyCart.CameraPosition = Mathf.Clamp(newPosition, 0f, _pathLength);
        }

        private void EdgeScrollWithKeyboard()
        {
            var horizontalInput = Input.GetAxis("Horizontal");

            if (!(Mathf.Abs(horizontalInput) > 0.01f)) return;
            var newPosition = _dollyCart.CameraPosition + horizontalInput * _moveSpeed * Time.deltaTime;
            _dollyCart.CameraPosition = Mathf.Clamp(newPosition, 0f, _pathLength);
        }
    }
}
