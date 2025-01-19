using System;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using RPG.Core;
using UnityEngine;

namespace RPG.Ability
{
    [CreateAssetMenu(fileName = "New Animation Trigger Effect", menuName = "Debdas/Abilities/Effect/Animation Trigger Effect")]
    public class AnimationTriggerEffect : EffectStrategy
    {
        [SerializeField] private string animationName = String.Empty;
        [SerializeField] private int layer = 0;

        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            Animator animator = data.GetUser().GetComponent<Animator>();

            DOVirtual.Float(0, 1, 0.1f, v =>
            {
                animator.SetLayerWeight(layer, v);
            });
            
            animator.PlayAnimation(animationName, 0.1f, layer, () =>
                {
                    DOVirtual.Float(1, 0, 0.1f, v =>
                    {
                        animator.SetLayerWeight(layer, v);
                    });
                    
                    onFinished?.Invoke();
                }, 0.8f
            );
        }
    }
}
