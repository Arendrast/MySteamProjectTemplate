using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations.Ragdoll
{
    public class EntryPoint : MonoBehaviour
    {
        private RagdollController _ragdollController;

        [SerializeField] private Shooter _shooter;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationPlayer _animationPlayer;

        [SerializeField] private int _backStandUpClipAnimationId;

        [SerializeField] [InlineButton(nameof(AppointBonesForBack))]
        private List<BoneTransformData> _bonesAtStartAnimationForBack;

        [SerializeField] private int _frontStandUpClipAnimationId;

        [SerializeField] [InlineButton(nameof(AppointBonesForFront))]
        private List<BoneTransformData> _bonesAtStartAnimationForFront;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PhysicsLayersConfig _physicsLayersConfig;

        private void Awake()
        {
            _ragdollController = new RagdollController(_animator,
                new RagdollStandUpController(transform, _animator, _backStandUpClipAnimationId, _frontStandUpClipAnimationId));

            _shooter.Construct(_ragdollController);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                _ragdollController.Fall(true);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                _ragdollController.StandUp(PlayStandUpAnimation);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                _ragdollController.AppointBonesToStartAnimation();
            }
        }

        private void AppointBonesForBack()
        {
            AppointBones(_bonesAtStartAnimationForBack);
        }

        private void AppointBonesForFront()
        {
            AppointBones(_bonesAtStartAnimationForFront);
        }

        private void AppointBones(List<BoneTransformData> array)
        {
            var hipsBone = _animator.GetBoneTransform(HumanBodyBones.Hips);
            var bones = hipsBone.GetComponentsInChildren<Transform>();

            array.Clear();

            foreach (var bone in bones)
            {
                array.Add(new BoneTransformData() { Position = bone.localPosition, Rotation = bone.localRotation });
            }
        }

        private void PlayStandUpAnimation(int id)
        {
            _animationPlayer.Play(id, 0);
        }
    }
}