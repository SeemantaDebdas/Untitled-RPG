using System;
using RPG.Core;
using UnityEngine;

namespace RPG.DialogueSystem
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;
        
        Animator animator;

        Interactable interactable;
        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if(interactable == null) 
                interactable = GetComponent<Interactable>();

            interactable.OnInteract += Interactable_OnInteract;
        }

        private void OnDisable()
        {
            interactable.OnInteract -= Interactable_OnInteract;
        }

        private void Interactable_OnInteract(Interactor interactor)
        {
            if (!interactor.TryGetComponent(out PlayerConversant playerConversant)) 
                return;
            
            playerConversant.StartConversation(dialogue);
            playerConversant.OnConversationUpdated += () =>
            {
                PlayerConversant_OnConversationUpdated(playerConversant);
            };
        }
        
        void PlayerConversant_OnConversationUpdated(PlayerConversant conversant)
        {
            if (!conversant.IsActive)
                return;
            
            Debug.Log(conversant.GetMood());
            switch (conversant.GetMood())
            {
                default:
                case DialogueNode.DialogueMood.NEUTRAL:
                    animator.PlayAnimation("NeutralTalk");
                    break;
                case DialogueNode.DialogueMood.HAPPY:
                    animator.PlayAnimation("HappyTalk");
                    break;
                case DialogueNode.DialogueMood.SAD:
                    animator.PlayAnimation("SadTalk");
                    break;
                case DialogueNode.DialogueMood.QUESTION:
                    animator.PlayAnimation("QuestionTalk");
                    break;
                case DialogueNode.DialogueMood.ANGRY:
                    animator.PlayAnimation("AngryTalk");
                    break;
                case DialogueNode.DialogueMood.SURPRISED:
                    animator.PlayAnimation("SurprisedTalk");
                    break;
            }
        }
    }
}
