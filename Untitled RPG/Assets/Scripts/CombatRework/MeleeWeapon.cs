using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat.Rework
{
    public class MeleeWeapon : Weapon
    {
        [SerializeField] private Collider weaponCollider;
        [SerializeField] private int damage = 10;

        [Header("Weapon Collision Points")] 
        [SerializeField] List<Transform> collisionPoints = new();

        [Header("Attacks")] 
        [SerializeField] private List<AttackData> attackList;

        private List<IDamageable> alreadyDamagedList = new();
        private List<Vector3> collisionPointsLastFrame = new();
        private RaycastHit[] hitArray = new RaycastHit[32];

        private IEnumerator colliderCoroutine = null;

        int currentAttackIndex = 0;

        AutoTimer timeSinceLastAttack;

        private bool isAttacking = false;
        
        private void Start()
        {
            weaponCollider.enabled = false;
            timeSinceLastAttack = new AutoTimer();
        }

        public override void Equip()
        {
            IsEquipped = true;
        }

        public override void Unequip()
        {
            IsEquipped = false;
        }

        public override void Attack(bool isHeavy)
        {
            isAttacking = true;
            //Debug.Break();
            currentAttackIndex = CanCombo() ? (currentAttackIndex + 1) % attackList.Count : 0;
            
            //animator.PlayAnimation(isHeavy ? "HeavyAttack" : "Light Attack", 0.1f, 4);
            animator.PlayAnimation(attackList[currentAttackIndex].AnimationName, 0.01f, AnimationLayer);

            if (colliderCoroutine != null)
            {
                StopCoroutine(colliderCoroutine); // ✅ Prevent duplicate coroutines
            }
            
            colliderCoroutine = SweepColliderPoints();
            StartCoroutine(colliderCoroutine);

            timeSinceLastAttack.SetTimeAndStartTimer(2f, () => {});
        }

        public override void Attack(CombatHandler target)
        {
        }

        public override void CounterAttack(CombatHandler target)
        {
        }

        public override bool CanCombo()
        {
            return !timeSinceLastAttack.IsOver() && isAttacking;
        }

        IEnumerator SweepColliderPoints()
        {
            yield return new WaitForEndOfFrame(); //for animator to successfully transition to next state. Crappy Unity Feature
            
            AttackData currentAttack = attackList[currentAttackIndex];
            collisionPointsLastFrame = collisionPoints.Select(point => point.position).ToList();
            
            yield return new WaitUntil(() => animator.GetNormalizedTime("Attack", layer: AnimationLayer) > currentAttack.ImpactStartTime);
        
        
            while (animator.GetNormalizedTime("Attack", layer: 4) <= currentAttack.ImpactEndTime)
            {
                for (int i = 0; i < collisionPoints.Count; i++)
                {
                    Vector3 dirPrevToCurr = collisionPoints[i].position - collisionPointsLastFrame[i];
                    dirPrevToCurr.Normalize();
                    collisionPointsLastFrame[i] = collisionPoints[i].position;
                    
                    Debug.DrawRay(collisionPoints[i].position, dirPrevToCurr, Color.red, 0.5f);
                    
                    int hits = Physics.SphereCastNonAlloc(collisionPoints[i].position, 
                        0.5f, dirPrevToCurr, 
                        hitArray, 
                        dirPrevToCurr.magnitude);

                    TryDamageColliders(hitArray, hits);
                }
        
                yield return null;
            }
            
            isAttacking = false; 
            colliderCoroutine = null;
            alreadyDamagedList.Clear();
        }

        void TryDamageColliders(RaycastHit[] hits, int hitCount)
        {
            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hitInfo = hits[i];
                
                GameObject hitObject = hitInfo.collider.gameObject;
                    
                if (!hitObject.TryGetComponent(out IDamageable damageable))
                    continue;
                    
                if (alreadyDamagedList.Contains(damageable))
                    continue;
            
                alreadyDamagedList.Add(damageable);

                Vector3 hitPoint = hitInfo.point;
                Vector3 hitDirection = (hitObject.transform.position - hitInfo.point).normalized;
            
                damageable.Damage(new(transform, damage, hitDirection, hitPoint));
            }
        }
    }

}
