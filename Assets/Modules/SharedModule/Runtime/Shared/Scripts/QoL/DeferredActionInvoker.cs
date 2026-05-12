using System;
using System.Collections;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class DeferredActionInvoker : IDisposable
    {
        public bool DidCallInvokeActionInThisFrame { get; private set; }
        public event Action InvokedOnEndFrame;
       
        public bool CanCallAction = true;
        
        private readonly CustomCoroutineRunner _coroutineRunner;

        public DeferredActionInvoker(CustomCoroutineRunner coroutineRunner) => _coroutineRunner = coroutineRunner;

        public void WaitEndFrameAndInvokeAction()
        {
            if (_coroutineRunner)
                _coroutineRunner.StartCoroutine(WaitEndFrameAndInvokeActionCoroutine());
        }

        public void Dispose()
        {
            InvokedOnEndFrame = null;
        }

        private IEnumerator WaitEndFrameAndInvokeActionCoroutine()
        {
            if (DidCallInvokeActionInThisFrame || !CanCallAction)
                yield break;

            DidCallInvokeActionInThisFrame = true;
            yield return new WaitForEndOfFrame();

            if (CanCallAction)
                InvokedOnEndFrame?.Invoke();

            DidCallInvokeActionInThisFrame = false;
        }
    }
}