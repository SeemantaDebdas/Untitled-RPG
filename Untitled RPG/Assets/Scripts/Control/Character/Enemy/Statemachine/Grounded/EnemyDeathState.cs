using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class EnemyDeathState : EnemyBaseState
    {
        [SerializeField] UnityEvent onEnter;
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(CharacterAnimationData.Instance.Hurt.Death, 0.1f);

            onEnter?.Invoke();
        }
    }
}
