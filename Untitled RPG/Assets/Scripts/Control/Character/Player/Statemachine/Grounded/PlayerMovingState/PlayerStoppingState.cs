using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerStoppingState : PlayerBaseState
    {
        [SerializeField] private string lightStopAnimation, hardStopAnimation;
        [SerializeField] private ScriptableFloat moveFloat, runFloat, sprintFloat;
        public override void Enter()
        {
            base.Enter();

            string animationName = "";
            //Debug.Log(moveFloat.Value + " " + sprintFloat.Value);
            Debug.Log(controller.velocity.magnitude);
            
            if (Mathf.Approximately(moveFloat.Value, runFloat.Value))
            {
                animationName = lightStopAnimation;
            }
            else if(Mathf.Approximately(moveFloat.Value, sprintFloat.Value))
            {
                animationName = hardStopAnimation;
            }

            animator.PlayAnimation(animationName, 0.1f);
        }
    }
}
