using System;
using BackEnd.Base_Classes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.Loaders
{
    public class LoadingScreenController : SingletonMonoBehaviour<LoadingScreenController>
    {
        private Animator _animator;
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            //print(GetAnimatorStateName());
        }

        public void StartAnimation()
        {
            _animator.SetTrigger("Close");
        }
        public void EndAnimation()
        {
            _animator.SetTrigger("Open");
        }
        public bool IsInState(string stateName)
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }
        private string GetAnimatorStateName()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName("DoorClosedIdle") ? "DoorClosedIdle" :
                _animator.GetCurrentAnimatorStateInfo(0).IsName("DoorCloses") ? "DoorCloses" :
                _animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen") ? "DoorOpen" :
                _animator.GetCurrentAnimatorStateInfo(0).IsName("DoorIdle") ? "DoorIdle" :
                "Unknown";
        }
        
        
        
        
        
    }
}