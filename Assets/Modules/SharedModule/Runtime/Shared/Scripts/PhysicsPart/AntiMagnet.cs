#if !TWO_D
using System.Collections.Generic;
using System.Linq;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart
{
    [RequireComponent(typeof(SphereCollider))]
    public class AntiMagnet : MonoBehaviour
    {
        [SerializeField] private float repelForce = 50f;

        [SerializeField] private AnimationCurve repelFalloff = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [SerializeField] private LayerMask targetLayers = -1;
        [SerializeField] private List<GameObject> exceptGameObjects;

        private HashSet<ManyInvokableOneFrameCharacterController> activeTargets = new();
        private SphereCollider repellerCollider;

        private ManyInvokableOneFrameCharacterController _manyInvokableOneFrameCharacterController;

        private void OnEnable()
        {
            repellerCollider = GetComponent<SphereCollider>();
            repellerCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var cc = other.GetComponent<ManyInvokableOneFrameCharacterController>();
            if (cc != null && targetLayers.DoesHaveLayer(other.gameObject.layer) &&
                exceptGameObjects.All(x => x != cc.gameObject))
            {
                activeTargets.Add(cc);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var cc = other.GetComponent<ManyInvokableOneFrameCharacterController>();
            if (cc != null)
            {
                activeTargets.Remove(cc);
            }
        }

        private void FixedUpdate()
        {
            foreach (var character in activeTargets)
            {
                if (character != null)
                {
                    ApplyRepelForce(character);
                }
            }

            activeTargets.RemoveWhere(cc => cc == null);
        }

        private void ApplyRepelForce(ManyInvokableOneFrameCharacterController characterController)
        {
            var characterPos = characterController.transform.position;
            var repellerPos = transform.position;
            var direction = (characterPos - repellerPos).normalized;
            var distance = Vector3.Distance(characterPos, repellerPos);

            if (distance < 0.01f)
                return;

            var normalizedDistance = Mathf.Clamp01(distance / repellerCollider.radius);
            var falloffFactor = repelFalloff.Evaluate(normalizedDistance);
            var force = repelForce * falloffFactor;

            var velocity = direction * force;
            var deltaTime = Time.fixedDeltaTime;

            characterController.Move(velocity * deltaTime);
        }

        public void SetRepelForce(float newForce) => repelForce = Mathf.Max(0, newForce);
    }
}
#endif