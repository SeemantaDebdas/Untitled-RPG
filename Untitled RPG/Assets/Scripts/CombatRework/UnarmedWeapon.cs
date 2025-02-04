using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat.Rework
{
    public class UnarmedWeapon : Weapon
    {
        [Header("Weapon Settings")]
        [SerializeField] private int damage = 10;
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private int animationLayer = 5;
        
        [Header("Weapon Collision Points")] 
        [SerializeField] private Transform leftFist;
        [SerializeField] private Transform rightFist;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;

        private Dictionary<AttackPoint, Transform> attackPointDictionary;
        
        [Header("Attacks")] 
        [SerializeField] private List<UnarmedAttackData> attackList = new();

        private List<IDamageable> alreadyDamagedList = new();
        private Collider[] hitArray = new Collider[32];

        private IEnumerator colliderCoroutine = null;

        private int currentAttackIndex = 0;
        private AutoTimer timeSinceLastAttack;
        
        bool canCombo = false;

        private void Start()
        {
            timeSinceLastAttack = new AutoTimer();

            // Initialize dictionary
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
                //return;
            }

            leftFist = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightFist = animator.GetBoneTransform(HumanBodyBones.RightHand);
            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);

            attackPointDictionary = new Dictionary<AttackPoint, Transform>
            {
                { AttackPoint.LeftFist, leftFist },
                { AttackPoint.RightFist, rightFist },
                { AttackPoint.LeftFoot, leftFoot },
                { AttackPoint.RightFoot, rightFoot }
            };

            Debug.Log("Unarmed attack points initialized.", this);
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
            currentAttackIndex = CanCombo() ? (currentAttackIndex + 1) % attackList.Count : 0;
            
            UnarmedAttackData currentAttack = attackList[currentAttackIndex];
            canCombo = false;

            // Play the specific animation for this attack
            animator.PlayAnimation(currentAttack.AnimationName, 0.01f, animationLayer);

            if (colliderCoroutine != null)
            {
                StopCoroutine(colliderCoroutine); // ✅ Prevent duplicate coroutines
            }
            
            colliderCoroutine = ActivateColliders(currentAttack);
            StartCoroutine(colliderCoroutine);

            timeSinceLastAttack.SetTimeAndStartTimer(2f, () => { });
        }

        public override bool CanCombo()
        {
            return !timeSinceLastAttack.IsOver() && canCombo;
        }

        private IEnumerator ActivateColliders(UnarmedAttackData attackData)
        {
            yield return new WaitForEndOfFrame(); // Allow animation transition
            
            yield return new WaitUntil(() => 
                animator.GetNormalizedTime("Attack", layer: animationLayer) > attackData.ImpactStartTime);

            Transform attackPoint = attackPointDictionary[attackData.attackPoint];

            while (animator.GetNormalizedTime("Attack", layer: animationLayer) <= attackData.ImpactEndTime)
            {
                int hits = Physics.OverlapSphereNonAlloc(attackPoint.position, attackData.attackRadius, hitArray, hitLayers);

                TryDamageColliders(hitArray, hits);

                yield return null;
            }
            
            colliderCoroutine = null;
            alreadyDamagedList.Clear();

            yield return new WaitUntil(() =>
                animator.GetNormalizedTime("Attack", layer: animationLayer) >= attackData.ComboTime);

            canCombo = true;
        }

        private void TryDamageColliders(Collider[] hits, int hitCount)
        {
            for (int i = 0; i < hitCount; i++)
            {
                GameObject hitObject = hits[i].gameObject;
            
                if (!hitObject.TryGetComponent(out IDamageable damageable))
                    continue;

                if (damageable.transform.root == transform.root)
                {
                    //Debug.Log("Trying to damage same damageable object!");
                    continue;
                }
            
                if (alreadyDamagedList.Contains(damageable))
                    continue;
    
                alreadyDamagedList.Add(damageable);

                Vector3 hitPoint = hits[i].ClosestPoint(transform.position);
                Vector3 hitDirection = (hitObject.transform.position - hitPoint).normalized;
    
                damageable.Damage(new(transform, damage, hitDirection, hitPoint));
            }
        }

    }
}
