using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Ui
{
    public class BirdRandomizer :MonoBehaviour
    {
        private Animator _animator;
        private RectTransform _transform;
        private float _randomAnimatorSpeed;
        private float _randomSpeed;
        private float _randomSize;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = GetComponent<RectTransform>();
            _randomAnimatorSpeed = Random.Range(0.6f, 1);
            _randomSpeed = Random.Range(30, 45);
            _randomSize = Random.Range(20, 40);
            SetRandomSize();
            _animator.speed = _randomAnimatorSpeed;
        }


        private void Update()
        {
            _transform.Translate(Vector3.left * _randomSpeed * Time.deltaTime);
            TeleportOffScreen();
        }

        private void TeleportOffScreen()
        {
            if (!(_transform.localPosition.x < -600)) return;
            _transform.localPosition = new Vector3(600 ,_transform.localPosition.y ,_transform.localPosition.z);
        }

        private void SetRandomSize()
        {
            _transform.sizeDelta = new Vector2(_randomSize, _randomSize);
        }
        
    }
}