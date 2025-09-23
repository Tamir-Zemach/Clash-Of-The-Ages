using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;


namespace BackEnd.Utilities
{
    public static class CameraShake
    {
        private static CinemachineCamera _virtualCamera;

        /// <summary>
        /// Initializes the camera reference. Call this once during setup.
        /// </summary>
        public static void Init(CinemachineCamera camera)
        {
            _virtualCamera = camera;
        }


        public static void Shake(float strength = 0.5f, float duration = 2, int vibrato = 10, float randomness = 90f)
        {
            if (_virtualCamera == null)
            {
                _virtualCamera = Object.FindAnyObjectByType<CinemachineCamera>();
            }

            if (_virtualCamera == null)
            {
                Debug.LogError("Cinemachine Camera not found");
                return;
            }
            

            _virtualCamera.transform.DOShakePosition(duration, strength, vibrato, randomness);
        }
    }
}