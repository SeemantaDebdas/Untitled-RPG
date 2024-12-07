using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Data;
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

        private IContextProvider contextProvider = null;

        private void Awake()
        {
            contextProvider = GetComponent<IContextProvider>(); 
        }

        public void StartConversation(Dialogue dialogue)
        {
            currentDialogue = dialogue;
            
            var rootNodeChildren = FilterOnCondition(currentDialogue.GetNonChoiceChildrenOfNode(currentDialogue.RootNode));
            currentDialogueNode = rootNodeChildren.FirstOrDefault();
            
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
            var choiceChildren = FilterOnCondition(currentDialogue.GetChoiceChildrenOfNode(currentDialogueNode)).ToList();

            if (choiceChildren.Count > 0)
            {
                OnConversationUpdated?.Invoke();
                return;
            }
            
            var nonChoiceChildren = FilterOnCondition(currentDialogue.GetNonChoiceChildrenOfNode(currentDialogueNode)).ToList();
            
            TriggerExitAction();
            
            currentDialogueNode = nonChoiceChildren[0];
            
            TriggerEnterAction();
            
            OnConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            List<DialogueNode> currentDialogueNodeChildren = FilterOnCondition(currentDialogue.GetChildrenOfNode(currentDialogueNode)).ToList();
            return currentDialogueNodeChildren.Count != 0;
        }

        public IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNodes)
        {
            foreach (DialogueNode dialogueNode in inputNodes)
            {
                if(dialogueNode.CheckCondition(contextProvider.GetContext()))
                    yield return dialogueNode;
            }
        }
        
        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetChoiceChildrenOfNode(currentDialogueNode));
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
            
            if (FilterOnCondition(currentDialogue.GetChoiceChildrenOfNode(currentDialogueNode)).Any())
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
