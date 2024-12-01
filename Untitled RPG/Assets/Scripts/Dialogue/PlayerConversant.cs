using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.DialogueSystem
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialogue;
        private DialogueNode currentDialogueNode = null;
        public event Action OnConversationUpdated, OnConversationEnded;
        
        public UnityEvent OnConversationStarted;
        
        public bool IsActive => currentDialogue != null;

        public void StartConversation(Dialogue dialogue)
        {
            currentDialogue = dialogue;
            currentDialogueNode = currentDialogue.RootNode;
            
            TriggerEnterAction();
            
            OnConversationUpdated?.Invoke();
            OnConversationStarted?.Invoke();
        }

        public void QuitConversation()
        {
            currentDialogue = null;
            
            TriggerExitAction();
            
            OnConversationUpdated?.Invoke();
            OnConversationEnded?.Invoke();
        }

        public string GetText()
        {
            if (currentDialogue == null || currentDialogueNode == null)
                return string.Empty;

            return currentDialogueNode.Text;
        } 

        public void Next()
        {
            if (currentDialogue.GetChoiceChildrenOfNode(currentDialogueNode).Any())
            {
                OnConversationUpdated?.Invoke();
                return;
            }
            
            List<DialogueNode> currentDialogueNodeChildren = currentDialogue.GetNonChoiceChildrenOfNode(currentDialogueNode).ToList();
            
            TriggerExitAction();
            
            currentDialogueNode = currentDialogueNodeChildren[0];
            
            TriggerEnterAction();
            
            OnConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            List<DialogueNode> currentDialogueNodeChildren = currentDialogue.GetChildrenOfNode(currentDialogueNode).ToList();
            return currentDialogueNodeChildren.Count != 0;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetChoiceChildrenOfNode(currentDialogueNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            TriggerExitAction();
            
            currentDialogueNode = chosenNode;
            
            TriggerEnterAction();
            
            Next();
        }

        public bool HasChoices()
        {
            if (currentDialogue == null)
                return false;
            
            if (currentDialogue.GetChoiceChildrenOfNode(currentDialogueNode).Any())
            {
                return true;
            }
            
            return false;
        }

        public void TriggerEnterAction()
        {
            if (currentDialogueNode == null)
                return;

            if (currentDialogueNode.OnEnterEvent == null)
                return;
            
            currentDialogueNode.OnEnterEvent.Raise(this, null);
        }
        
        public void TriggerExitAction()
        {
            if (currentDialogueNode == null)
                return;

            if (currentDialogueNode.OnExitEvent == null)
                return;
            
            currentDialogueNode.OnExitEvent.Raise(this, null);
        }
    }
}
