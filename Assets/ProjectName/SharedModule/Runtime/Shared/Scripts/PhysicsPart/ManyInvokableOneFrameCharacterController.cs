using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart
{
    public class ManyInvokableOneFrameCharacterController : MonoBehaviour
    {
        public CharacterController Controller { get; private set; }
        
        private Vector3 _resultDirection;

        private DeferredActionInvoker _deferredActionInvoker;

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
            _deferredActionInvoker = new DeferredActionInvoker(gameObject.GetOrAddComponent<CustomCoroutineRunner>());
            _deferredActionInvoker.InvokedOnEndFrame += FinalMove;
        }

        public void OnDestroy()
        {
            _deferredActionInvoker?.Dispose();
        }

        public void Move(Vector3 direction)
        {
            _resultDirection += direction;
            
            _deferredActionInvoker.WaitEndFrameAndInvokeAction();
        }

        private void FinalMove()
        {
            Controller.Move(_resultDirection);
            _resultDirection = Vector3.zero;
        }
    }
}