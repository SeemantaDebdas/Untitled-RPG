using UnityEngine;

namespace RPG.Core
{
    public class NPCAnimationPlayer : MonoBehaviour
    {
        [SerializeField] string animationName = "";
        Animator animator;
        void Awake()
        {
            animator = GetComponent<Animator>();

            animator.Play(animationName);
        }
    }
}
