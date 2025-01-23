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
        [SerializeField] List<AttackData> attackList = new();

        private List<IDamageable> alreadyDamagedList = new();
        private List<Vector3> collisionPointsLastFrame = new();

        private IEnumerator colliderCoroutine = null;

        int currentAttackIndex = 0;

        AutoTimer timeSinceLastAttack;
        
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
            //Debug.Break();
            currentAttackIndex = CanCombo() ? (currentAttackIndex + 1) % attackList.Count : 0;
            
            //animator.PlayAnimation(isHeavy ? "HeavyAttack" : "Light Attack", 0.1f, 4);
            animator.PlayAnimation(attackList[currentAttackIndex].AnimationName, 0.01f, 4);

            colliderCoroutine = SweepColliderPoints();
            StartCoroutine(colliderCoroutine);

            timeSinceLastAttack.SetTimeAndStartTimer(2f, () => { });
        }

        public override bool CanCombo()
        {
            if (timeSinceLastAttack.IsOver())
                return false;

            if (weaponCollider.enabled)
                return false;

            return true;
        }

        IEnumerator EnableAndDisableCollider()
        {
            AttackData currentAttack = attackList[currentAttackIndex];
            
            yield return new WaitUntil(() => animator.GetNormalizedTime("Attack", layer: 4) > currentAttack.ImpactStartTime);
            weaponCollider.enabled = true;

            yield return new WaitUntil(() => animator.GetNormalizedTime("Attack", layer: 4) > currentAttack.ImpactEndTime);
            weaponCollider.enabled = false;

            colliderCoroutine = null;
            alreadyDamagedList.Clear();
        }
        

        IEnumerator SweepColliderPoints()
        {
            yield return new WaitForEndOfFrame(); //for animator to successfully transition to next state. Crappy Unity Feature
            
            AttackData currentAttack = attackList[currentAttackIndex];
            collisionPointsLastFrame = collisionPoints.Select(point => point.position).ToList();
            
            yield return new WaitUntil(() => animator.GetNormalizedTime("Attack", layer: 4) > currentAttack.ImpactStartTime);
        
        
            while (animator.GetNormalizedTime("Attack", layer: 4) <= currentAttack.ImpactEndTime)
            {
                for (int i = 0; i < collisionPoints.Count; i++)
                {
                    Vector3 dirFromCurrentToPreviousPosition = collisionPointsLastFrame[i] - collisionPoints[i].position;
                    dirFromCurrentToPreviousPosition.Normalize();
                    collisionPointsLastFrame[i] = collisionPoints[i].position;
                    
                    Debug.DrawRay(collisionPoints[i].position, dirFromCurrentToPreviousPosition, Color.red, 4);

                    if (!Physics.Raycast(collisionPoints[i].position, dirFromCurrentToPreviousPosition,
                            out RaycastHit hitInfo, dirFromCurrentToPreviousPosition.magnitude))
                        continue;
                    
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
        
                yield return null;
            }
            
            //yield return new WaitUntil(() => animator.GetNormalizedTime("Attack", layer: 4) > currentAttack.ImpactEndTime);
            colliderCoroutine = null;
            alreadyDamagedList.Clear();
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     Debug.Log(attackList[currentAttackIndex].AnimationName + " " + other.name);
        //     
        //     if (other.TryGetComponent(out IDamageable damageable))
        //     {
        //         if (alreadyDamagedList.Contains(damageable))
        //             return;
        //         
        //         alreadyDamagedList.Add(damageable);
        //         
        //         Vector3 hitPoint = other.ClosestPoint(transform.position);
        //         Vector3 hitDirection = (other.transform.position - transform.position).normalized;
        //         
        //         damageable.Damage(new(transform, damage, hitDirection, hitPoint));
        //     }
        // }
    }

}
