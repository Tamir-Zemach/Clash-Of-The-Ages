using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using Managers;
using UnityEngine;

namespace BackEnd.Utilities
{
    public class CoroutineManager : OneInstanceMonoBehaviour<CoroutineManager>
    {
        private readonly List<ManagedCoroutine> _coroutines = new List<ManagedCoroutine>();

        public ManagedCoroutine StartManagedCoroutine(IEnumerator routine)
        {
            var managed = new ManagedCoroutine(routine, this);
            _coroutines.Add(managed);
            managed.Start();
            return managed;
        }

        protected override void Awake()
        {
            base.Awake();
            GameStates.Instance.OnGamePaused += PauseAll;
            GameStates.Instance.OnGameResumed += ResumeAll;
            GameStates.Instance.OnGameEnded += StopAll;
            GameStates.Instance.OnGameReset += StopAll;
        }

        private void OnDestroy()
        {
            GameStates.Instance.OnGamePaused -= PauseAll;
            GameStates.Instance.OnGameResumed -= ResumeAll;
            GameStates.Instance.OnGameEnded -= StopAll;
            GameStates.Instance.OnGameReset -= StopAll;
        }

        public void PauseAll()
        {
            foreach (var coroutine in _coroutines)
                coroutine.Pause();
        }

        public void ResumeAll()
        {
            foreach (var coroutine in _coroutines)
                coroutine.Resume();
        }

        public void StopAll()
        {
            foreach (var coroutine in _coroutines)
                coroutine.Stop();
            _coroutines.Clear();
        }
    }
}