using DG.Tweening;
using RPG.Combat;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RPG.Control
{
    public class PlayerRangedDrawState : PlayerBaseState
    {
        //move this to SO variables later
        [Header("ANIMATION")]
        [SerializeField] string strafe = string.Empty;
        [SerializeField] string moveXParam, moveYParam;

        [Header("SHOOT PARAMS")]
        [SerializeField, Range(0.0f, 50.0f)] float maxForce;
        [SerializeField] float speedToReachMaxForce = 5f;

        [Header("STATE PARAMS")]
        [SerializeField] float speed = 5f;
        [SerializeField] float rotationSpeed = 180f;

        [Header("RIGS")]
        [SerializeField] List<MultiAimConstraint> aimRigs;

        RangedWeaponSO rangedWeapon;
        TrajectoryPredictor trajectory;
        ProjectileThrower thrower;

        float currentForce;

        public override void Enter()
        {
            base.Enter();

            currentForce = 0;
            trajectory = GetComponent<TrajectoryPredictor>();

            thrower = GetComponentInParent<ProjectileThrower>();

            rangedWeapon = weaponHandler.CurrentWeapon as RangedWeaponSO;


            //This can be passed on to scriptable object, weaponSO by a function. Maybe SetWeaponLayerWeight(Animator animator)...
            animator.SetLayerWeightOverTime(0, 0.1f, rangedWeapon.UnsheathAnimationLayer);
            animator.SetLayerWeightOverTime(1, 0.1f, rangedWeapon.DrawFireAnimationLayer);
            animator.PlayAnimation(rangedWeapon.DrawAnimation, 0.1f, rangedWeapon.DrawFireAnimationLayer);
            animator.PlayAnimation(strafe);
            
            trajectory.EnableVisual();

            foreach(var aimRig in aimRigs)
                DOVirtual.Float(0, 1, 0.1f, (v) => aimRig.weight = v);
        }

        public override void Exit()
        {
            base.Exit();

            trajectory.DisableVisual();
            thrower.SetForce(currentForce);

            foreach (var aimRig in aimRigs)
                DOVirtual.Float(1, 0, 0.1f, (v) => aimRig.weight = v);
        }

        public override void Tick()
        {
            base.Tick();
            
            currentForce = Mathf.Min(currentForce + Time.deltaTime * speedToReachMaxForce, maxForce);
            
            trajectory.PredictTrajectory(GetProjectileData());
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

            Vector3 lookAt = ray.GetPoint(maxForce);

            //RaycastHit[] raycastHits = new RaycastHit[20];
            ////int hits = Physics.RaycastNonAlloc(ray, raycastHits, maxRange, shootLayer, QueryTriggerInteraction.Ignore);

            ////for (int i = 0; i < hits; i++)
            ////{
            ////    var hit = raycastHits[i];
            ////    if (hit.collider.CompareTag("Player"))//change this with scriptable object variable
            ////    {
            ////        continue;
            ////    }

            ////    lookAt = hit.transform.position;
            ////    break;
            ////}

            Vector3 lookDirection = (lookAt - context.Transform.position).normalized;
            lookDirection.y = 0;

            context.Transform.rotation = Quaternion.Slerp(context.Transform.rotation,
                                    Quaternion.LookRotation(lookDirection),
                                    rotationSpeed * Time.deltaTime);
        }

        ProjectileProperties GetProjectileData()
        {
            Weapon currentWeaponInstance = weaponHandler.CurrentWeaponInstance;

            ProjectileProperties properties = new ProjectileProperties();
            Rigidbody projectile = rangedWeapon.Projectile.GetComponent<Rigidbody>();

            properties.direction = currentWeaponInstance.ShootPoint.forward;
            properties.initialPosition = currentWeaponInstance.ShootPoint.position;
            properties.initialSpeed = currentForce;
            properties.mass = projectile.mass;
            properties.drag = projectile.drag;

            return properties;
        }
    }
}
