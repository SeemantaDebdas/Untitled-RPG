using DG.Tweening;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public static class AnimatorExtension
    {
        public static void PlayAnimation(this Animator animator, string animationName, float transitionDuration = 0.25f, int layer = 0, Action onAnimationEnd = null, float triggerTime = 1f)
        {
            animator.CrossFadeInFixedTime(animationName, transitionDuration, layer);
            if (onAnimationEnd != null)
            {
                Timing.RunCoroutine(TrackAnimation(animator, animationName, layer, triggerTime, onAnimationEnd));
            }
        }

        public static float GetNormalizedTime(this Animator animator, string tag, int layer = 0)
        {
            AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(layer);

            if (animator.IsInTransition(layer) && nextStateInfo.IsTag(tag))
            {
                return nextStateInfo.normalizedTime;
            }

            if (!animator.IsInTransition(layer) && currentStateInfo.IsTag(tag))
            {
                return currentStateInfo.normalizedTime;
            }

            return 0;
        }

        private static IEnumerator<float> TrackAnimation(Animator animator, string animationName, int layer, float triggerTime, Action onAnimationEnd)
        {
            while (true)
            {
                AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

                // Check if the current animation is the one we want to track
                if (currentStateInfo.IsName(animationName))
                {
                    // If the normalized time reaches the specified trigger time, call the action
                    if (currentStateInfo.normalizedTime >= triggerTime)
                    {
                        onAnimationEnd?.Invoke();
                        yield break; // Stop tracking after the action is called
                    }
                }

                yield return Timing.WaitForOneFrame; // Wait until the next frame
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

        public static void SetFloatValueOverTime(this Animator animator, string parameter, float to, float time = 0.1f)
        {
            DOVirtual.Float(animator.GetFloat(parameter), to, time, (v) =>
            {
                animator.SetFloat(parameter, v);
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
