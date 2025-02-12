using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RPG.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat.Rework
{
    public class UnarmedWeapon : Weapon
    {
        [Header("Weapon Settings")]
        [SerializeField] private int baseDamage = 10;
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private int animationLayer = 5;
        
        [Header("Weapon Collision Points")] 
        [SerializeField] private Transform leftFist;
        [SerializeField] private Transform rightFist;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;

        private Dictionary<AttackPoint, Transform> attackPointDictionary;

        [Header("Attacks")] 
        [SerializeField] private float farAttackThreshold = 2f;
        [SerializeField] private AttackCombo closeAttackCombo;
        [SerializeField] private AttackCombo farAttackCombo;
        [SerializeField] private AttackCombo knockoutCombo;
        [SerializeField] private UnarmedAttackData counterAttackData;

        private List<IDamageable> alreadyDamagedList = new();
        private Collider[] hitArray = new Collider[32];

        private IEnumerator colliderCoroutine, notifyAttackToTargetCoroutine = null;
        private int currentAttackIndex = 0;
        private AutoTimer timeSinceLastAttack;
        private bool canCombo = false;
        private bool lastAttackWasCloseAttack = true;

        private CombatHandler attackTarget = null;
        

        private void Start()
        {
            timeSinceLastAttack = new AutoTimer();
            InitializeAttackPointDictionary();
        }

        private void InitializeAttackPointDictionary()
        {
            attackPointDictionary = new Dictionary<AttackPoint, Transform>
            {
                { AttackPoint.LeftFist, leftFist },
                { AttackPoint.RightFist, rightFist },
                { AttackPoint.LeftFoot, leftFoot },
                { AttackPoint.RightFoot, rightFoot }
            };
        }

        [ContextMenu("Setup Attack Points")]
        private void InitializeAttackPoints()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                Debug.LogError("Animator is not assigned!", this);
            }

            leftFist = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightFist = animator.GetBoneTransform(HumanBodyBones.RightHand);
            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            
            InitializeAttackPointDictionary();
            Debug.Log("Unarmed attack points initialized.", this);
        }

        public override void Equip() => IsEquipped = true;
        public override void Unequip() => IsEquipped = false;
        
        public override void Attack(bool isHeavy)
        {
            var currentAttack = GetAttackData();
            ExecuteAttack(currentAttack);
        }

        public override void Attack(CombatHandler target)
        {
            attackTarget = target;
            
            var currentAttack = GetAttackData();
            transform.LookAt(attackTarget.transform);
            
            ExecuteAttack(currentAttack);

            notifyAttackToTargetCoroutine = NotifyAttackToTarget(target, currentAttack);
            StartCoroutine(notifyAttackToTargetCoroutine);
        }

        public override void CounterAttack(CombatHandler target)
        {
            attackTarget = target;
            transform.LookAt(attackTarget.transform);
            
            ExecuteAttack(counterAttackData);

            notifyAttackToTargetCoroutine = NotifyAttackToTarget(target, counterAttackData);
            StartCoroutine(notifyAttackToTargetCoroutine);
        }

        private IEnumerator NotifyAttackToTarget(CombatHandler target, UnarmedAttackData currentAttack)
        {
            yield return null;
            
            Transform weaponHolder = transform.root;
            
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            float timeTillImpact = Time.time + (animationLength * currentAttack.ImpactStartTime);
            
            target.NotifyAttack(weaponHolder, currentAttack, timeTillImpact);
        }

        private void ExecuteAttack(UnarmedAttackData attackData)
        {
            canCombo = false;
            
            animator.PlayAnimation(attackData.AnimationName, 0.01f, animationLayer);
            RestartColliderCoroutine(attackData);
            timeSinceLastAttack.SetTimeAndStartTimer(2f, () => { });
        }

        private UnarmedAttackData GetAttackData()
        {
            AttackCombo attackCombo = closeAttackCombo;
            
            if (attackTarget != null)
            {
                if (attackTarget.IsStunned)
                {
                    return knockoutCombo.GetRandomAttack() as UnarmedAttackData;
                }
                
                float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
                if (distance >= farAttackThreshold && farAttackCombo.HasAttacks())
                    attackCombo = farAttackCombo;
            }

            UnarmedAttackData currentAttack = CanCombo()
                ? attackCombo.GetNextAttack() as UnarmedAttackData
                : attackCombo.ResetCombo() as UnarmedAttackData;
            
            return currentAttack;
        }

        private void RestartColliderCoroutine(UnarmedAttackData attackData)
        {
            if (colliderCoroutine != null)
            {
                StopCoroutine(colliderCoroutine);
            }
            colliderCoroutine = ActivateColliders(attackData);
            StartCoroutine(colliderCoroutine);
        }

        public override bool CanCombo()
        {
            return !timeSinceLastAttack.IsOver() && canCombo;
        }

        private IEnumerator ActivateColliders(UnarmedAttackData attackData)
        {
            yield return new WaitForEndOfFrame(); // Ensure animation has transitioned

            if (attackTarget != null)
            {
                MoveToAttackTarget(attackData);
            }

            yield return WaitForImpactStart(attackData);

            yield return ApplyDamageDuringImpact(attackData);

            yield return WaitForComboTime(attackData);

            yield return WaitForAnimationEnd();

            ResetAfterAttack();
        }

        private void ResetAfterAttack()
        {
            attackTarget = null;
            animator.applyRootMotion = false;
            Array.Clear(hitArray, 0, hitArray.Length);
        }

        private IEnumerator WaitForImpactStart(UnarmedAttackData attackData)
        {
            yield return new WaitUntil(() =>
                animator.GetNormalizedTime("Attack", animationLayer) > attackData.ImpactStartTime);
        }

        private IEnumerator ApplyDamageDuringImpact(UnarmedAttackData attackData)
        {
            Transform attackPoint = attackPointDictionary[attackData.attackPoint];

            while (animator.GetNormalizedTime("Attack", animationLayer) <= attackData.ImpactEndTime)
            {
                int hits = Physics.OverlapSphereNonAlloc(
                    attackPoint.position, attackData.attackRadius, hitArray, hitLayers);
                
                TryDamageColliders(hitArray, hits, attackData);
                yield return null;
            }

            colliderCoroutine = null;
            alreadyDamagedList.Clear();
        }

        private IEnumerator WaitForComboTime(UnarmedAttackData attackData)
        {
            yield return new WaitUntil(() =>
                animator.GetNormalizedTime("Attack", animationLayer) >= attackData.ComboTime);
            canCombo = true;
        }

        private IEnumerator WaitForAnimationEnd()
        {
            yield return new WaitUntil(() =>
                animator.GetNormalizedTime("Attack", animationLayer) >= 0.9f);
        }

        private Tween moveTween; 
        private void MoveToAttackTarget(UnarmedAttackData attackData)
        {
            if (attackTarget == null) return;

            float impactStartTime = attackData.ImpactStartTime; // Get normalized time for impact
            float animationDuration = animator.GetCurrentAnimatorStateInfo(animationLayer).length;
            float moveDuration = impactStartTime * animationDuration - 0.1f; // Convert normalized time to seconds
            
            // Calculate the position 1 unit away from the target, facing the enemy
            Vector3 targetPosition = attackTarget.transform.position + (transform.position - attackTarget.transform.position).normalized * attackData.DistanceFromTarget;

            // Stop any existing movement
            moveTween?.Kill();

            // Move to the target position smoothly
            moveTween = transform.DOMove(targetPosition, moveDuration) // Adjust duration as needed
                .SetEase(Ease.OutSine);
        }

        private void TryDamageColliders(Collider[] hits, int hitCount, UnarmedAttackData attackData)
        {
            for (int i = 0; i < hitCount; i++)
            {
                GameObject hitObject = hits[i].gameObject;
                
                if (!hitObject.TryGetComponent(out IDamageable damageable))
                    continue;

                if (damageable.transform.root == transform.root || alreadyDamagedList.Contains(damageable))
                {
                    Debug.Log("Already damaged: " + hitObject.name);
                    continue;
                }
                
                Debug.Log(hitObject.name);

                alreadyDamagedList.Add(damageable);

                Vector3 hitPoint = hits[i].ClosestPoint(transform.position);
                Vector3 hitDirection = (hitObject.transform.position - hitPoint).normalized;
    
                damageable.Damage(new(transform, attackData.Damage, hitDirection, hitPoint));
            }
        }
    }
}
