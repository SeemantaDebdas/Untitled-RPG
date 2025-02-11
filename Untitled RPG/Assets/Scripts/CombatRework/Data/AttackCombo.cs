using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Rework
{
    [System.Serializable]
    public class AttackCombo
    {
        [SerializeField] private List<AttackData> attackList;
        private int currentAttackIndex = 0;

        public AttackData GetNextAttack()
        {
            currentAttackIndex = (currentAttackIndex + 1) >= attackList.Count ? 0 : ++currentAttackIndex;
            return attackList[currentAttackIndex];
        }

        public AttackData ResetCombo()
        {
            currentAttackIndex = 0;
            return attackList[currentAttackIndex];
        }

        public bool HasAttacks() => attackList != null && attackList.Count != 0;
    }
}