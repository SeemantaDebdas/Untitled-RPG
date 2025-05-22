using System;
using System.Collections.Generic;
using RPG.Inventory.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Inventory.Pickup
{
    public class PickupSystem : MonoBehaviour
    {
        [SerializeField] private InputActionReference pickupAction = null;
        [SerializeField] private GridInventorySO gridInventory;
        [SerializeField] private List<Pickup> pickupList = new();
        
        public event Action<List<Pickup>> OnPickupListChanged; 
        
        private void OnEnable()
        {
            pickupAction.action.performed += context =>
            {
                if (pickupList.Count <= 0) return;
                
                Pickup pickup = pickupList[0];
                gridInventory.AddItem(pickup.Item, pickup.Quantity);
                pickupList.RemoveAt(0);
                OnPickupListChanged?.Invoke(pickupList);
                Destroy(pickup.gameObject);
            };
            pickupAction.action.Enable();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Pickup pickup))
                return;
            
            print(other.name);
            pickupList.Add(pickup);
            
            OnPickupListChanged?.Invoke(pickupList);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Pickup pickup))
                return;
            
            print(other.name);
            pickupList.Remove(pickup);
            
            OnPickupListChanged?.Invoke(pickupList);
        }
    }
}
