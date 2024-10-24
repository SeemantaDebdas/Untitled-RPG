using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerRollState : PlayerBaseState
    {
        [SerializeField] float speed = 1f;
        [SerializeField] float rotationSpeed = 20f;
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.Roll, 0.1f);
        }

        public override void Tick()
        {
            base.Tick();

            HandleMovement(speed);
            FaceDirection(CalculateDirection(), rotationSpeed);
        }
    }
}
