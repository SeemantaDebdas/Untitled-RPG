using System;
using RPG.Core;
using UnityEngine;

namespace RPG.DialogueSystem
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;

        Interactable interactable;
        private void Awake()
        {
            interactable = GetComponent<Interactable>();
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
            if(interactor.TryGetComponent(out PlayerConversant playerConversant))
            {
                playerConversant.StartConversation(dialogue);
            }
        }
    }
}
