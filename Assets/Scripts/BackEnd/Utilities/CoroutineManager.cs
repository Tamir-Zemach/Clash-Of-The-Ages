using System.Collections;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using UnityEngine;

namespace BackEnd.Utilities
{
    public class CoroutineManager : PersistentMonoBehaviour<CoroutineManager>
    {
        private readonly List<ManagedCoroutine> _coroutines = new List<ManagedCoroutine>();

        public ManagedCoroutine StartManagedCoroutine(IEnumerator routine)
        {
            var managed = new ManagedCoroutine(routine, this);
            _coroutines.Add(managed);
            managed.Start();
            return managed;
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