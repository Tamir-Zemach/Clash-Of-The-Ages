using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace BackEnd.Utilities
{
    public class ManagedCoroutine
    {
        private readonly IEnumerator _routine;
        private readonly MonoBehaviour _owner;
        
        private Coroutine _coroutine;
        private bool _isPaused;

        public ManagedCoroutine(IEnumerator routine, MonoBehaviour owner)
        {
            _routine = routine;
            _owner = owner;
        }

        public void Start()
        {
            _coroutine = _owner.StartCoroutine(Run());
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        public void Stop()
        {
            if (_coroutine != null || _owner)
            {
                _owner.StopCoroutine(_coroutine);
            }
        }

        private IEnumerator Run()
        {
            while (_routine.MoveNext())
            {
                while (_isPaused)
                    yield return null;

                yield return _routine.Current;
            }
        }
    }
}