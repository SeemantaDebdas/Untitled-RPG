using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.DialogueSystem
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialogue;
        private DialogueNode currentDialogueNode = null;
        public event Action OnConversationUpdated, OnConversationEnded;
        
        public bool HasChoices { get; private set; }
        public bool IsActive => currentDialogue != null;

        public void StartConversation(Dialogue dialogue)
        {
            currentDialogue = dialogue;
            currentDialogueNode = currentDialogue.RootNode;
            OnConversationUpdated?.Invoke();
        }

        public void QuitConversation()
        {
            currentDialogue = null;
            HasChoices = false;
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
                HasChoices = true;
                OnConversationUpdated?.Invoke();
                return;
            }
            
            List<DialogueNode> currentDialogueNodeChildren = currentDialogue.GetNonChoiceChildrenOfNode(currentDialogueNode).ToList();
            currentDialogueNode = currentDialogueNodeChildren[0];
            
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
            HasChoices = false;
            currentDialogueNode = chosenNode;
            Next();
        }
    }
}
