using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Core
{
    public static class AnimatorExtension
    {
        public static void PlayAnimation(this Animator animator, string animationName, float transitionDuration = 0.25f, int layer = 0)
        {
            animator.CrossFadeInFixedTime(animationName, transitionDuration, layer);
        }

        public static float GetNormalizedTime(this Animator animator, string tag, int layer = 0)
        {
            AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(layer);

            if (animator.IsInTransition(layer) && nextStateInfo.IsTag(tag))
            {
                return nextStateInfo.normalizedTime;
            }
            else if (!animator.IsInTransition(layer) && currentStateInfo.IsTag(tag))
            {
                return currentStateInfo.normalizedTime;
            }
            else
            {
                return 0;
            }
        }

        public static void CustomMatchTarget(this Animator animator, Vector3 matchPosition, Quaternion matchRotation, AvatarTarget target, MatchTargetWeightMask weightMask, float normalisedStartTime, float normalisedEndTime)
        {
            if (animator.IsInTransition(0))
            {
                return;
            }

            //Debug.Log("Not is in transition");

            if (animator.isMatchingTarget)
            {
                return;
            }

            //Debug.Log("Matching target");

            float normalizeTime = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);

            if (normalizeTime > normalisedEndTime)
            {
                return;
            }

            animator.MatchTarget(matchPosition, matchRotation, target, weightMask, normalisedStartTime, normalisedEndTime);
        }

        public static void SetLayerWeightOverTime(this Animator animator, float from, float to, float time = 0.1f, int layer = 0)
        {
            DOVirtual.Float(from, to, time, (v) =>
            {
                animator.SetLayerWeight(layer, v);
            });
        }

        public static void SetLayerWeightOverTime(this Animator animator, float to, float time = 0.1f, int layer = 0)
        {
            float from = animator.GetLayerWeight(layer);

            DOVirtual.Float(from, to, time, (v) =>
            {
                animator.SetLayerWeight(layer, v);
            });
        }
    }

}
