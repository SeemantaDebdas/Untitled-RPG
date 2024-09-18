using RPG.Combat;
using RPG.Control;
using RPG.Core;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerRangedDrawState : PlayerBaseState
    {
        //move this to SO variables later
        [Header("ANIMATION")]
        [SerializeField] string strafe = string.Empty;
        [SerializeField] string moveXParam, moveYParam;

        [Space]
        [SerializeField] float maxRange = 50f;
        [SerializeField] LayerMask shootLayer;
        [SerializeField] float speed = 5f;
        [SerializeField] float rotationSpeed = 180f;
        RangedWeaponSO rangedWeapon;
        public override void Enter()
        {
            base.Enter();

            rangedWeapon = weaponHandler.CurrentWeapon as RangedWeaponSO;


            //This can be passed on to scriptable object, weaponSO by a function. Maybe SetWeaponLayerWeight(Animator animator)...
            animator.SetLayerWeightOverTime(0, 0.1f, rangedWeapon.UnsheathAnimationLayer);
            animator.SetLayerWeightOverTime(1, 0.1f, rangedWeapon.DrawFireAnimationLayer);
            animator.PlayAnimation(rangedWeapon.DrawAnimation, 0.1f, rangedWeapon.DrawFireAnimationLayer);
            animator.PlayAnimation(strafe);
        }

        public override void Tick()
        {
            base.Tick();
            
            LookAtCenterOfScreen();
            HandleMovement(speed);
            HandleAnimation();
        }

        private void HandleAnimation()
        {
            animator.SetFloat(moveXParam, input.MoveInput.x, 0.1f, Time.deltaTime);
            animator.SetFloat(moveYParam, input.MoveInput.y, 0.1f, Time.deltaTime);
        }

        private void LookAtCenterOfScreen()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Vector3 lookAt = ray.GetPoint(maxRange);

            RaycastHit[] raycastHits = new RaycastHit[20];
            int hits = Physics.RaycastNonAlloc(ray, raycastHits, maxRange, shootLayer, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hits; i++)
            {
                var hit = raycastHits[i];
                if (hit.collider.CompareTag("Player"))//change this with scriptable object variable
                {
                    continue;
                }

                lookAt = hit.transform.position;
                break;
            }

            Vector3 lookDirection = (lookAt - context.Transform.position).normalized;

            context.Transform.rotation = Quaternion.Slerp(context.Transform.rotation,
                                    Quaternion.LookRotation(lookDirection),
                                    rotationSpeed * Time.deltaTime);
        }
    }
}
