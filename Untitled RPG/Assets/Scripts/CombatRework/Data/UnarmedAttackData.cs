using UnityEngine;

namespace RPG.Combat.Rework
{
    public enum AttackPoint
    {
        Root = 0,
        LeftFist = 1,
        RightFist = 2,
        LeftFoot = 3,
        RightFoot = 4
    }

    [CreateAssetMenu(fileName = "New Unarmed Attack Data", menuName = "Debdas/Combat/Unarmed Attack")]
    public class UnarmedAttackData : AttackData
    {
        public AttackPoint attackPoint; // Determines which body part is used
        public float attackRadius = 0.5f; // Radius for OverlapSphere
    }
}
