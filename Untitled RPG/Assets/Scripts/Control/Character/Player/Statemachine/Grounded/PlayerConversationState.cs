using RPG.Camera;
using RPG.Core;
using RPG.Data;
using UnityEngine;
using UnityEngine.Events;
using CameraType = RPG.Camera.CameraType;

namespace RPG.Control
{
    public class PlayerConversationState : PlayerBaseState
    {
        //call swtich state from the inspector
        [SerializeField] private UnityEvent OnConversationEnded;
        public override void Enter()
        {
            base.Enter();
            conversant.OnConversationEnded += PlayerConversant_OnConversationEnded;
            animator.PlayAnimation(CharacterAnimationData.Instance.Conversation.IdleArmsCrossed);
            weaponHandler.enabled = false;
            
            CameraController.Instance.SetHigherPriority(CameraType.DIALOGUE);
        }

        public override void Exit()
        {
            base.Exit();
            
            conversant.OnConversationEnded -= PlayerConversant_OnConversationEnded;
            weaponHandler.enabled = true;
            
            CameraController.Instance.SetHigherPriority(CameraType.FREE_LOOK);
        }

        private void PlayerConversant_OnConversationEnded()
        {
            OnConversationEnded?.Invoke();
        }

        protected override void InputReader_OnAttackCancelled(){}
        protected override void InputReader_OnHolsterPerformed(){}
    }
}
