using System;
using RPG.Core;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Inventory.Pickup
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private InventoryItemSO item = null;
        [SerializeField] private int quantity = 1;
        [SerializeField] InventorySO playerInventory = null;

        private Interactable interactable;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
        }

        private void OnEnable()
        {
            interactable.OnInteract += Interactable_OnInteract;
        }

        private void OnDisable()
        {
            interactable.OnInteract -= Interactable_OnInteract;
        }

        private void Interactable_OnInteract(Interactor interactor)
        {
            playerInventory.AddItem(item, quantity);
            Destroy(gameObject);
        }
    }
}