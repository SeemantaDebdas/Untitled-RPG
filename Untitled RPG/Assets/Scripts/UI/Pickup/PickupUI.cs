using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory.Pickup.UI
{
    public class PickupUI : MonoBehaviour
    {
        [SerializeField] private PickupSystem pickupSystem = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Transform pickupItemUIContainer = null;
        [SerializeField] PickupItemUI pickupItemUIPerfab = null;

        private void Start()
        {
            pickupSystem.OnPickupListChanged += PickupSystem_OnPickupListChanged;
            backgroundImage.enabled = false;
        }
        
        private void OnDestroy()
        {
            pickupSystem.OnPickupListChanged -= PickupSystem_OnPickupListChanged;
        }

        private void PickupSystem_OnPickupListChanged(List<Pickup> pickupList)
        {
            backgroundImage.enabled = pickupList.Count > 0;
            
            for (int i = 0; i < pickupItemUIContainer.childCount; i++)
            {
                Destroy(pickupItemUIContainer.GetChild(i).gameObject);
            }
            
            foreach (Pickup pickup in pickupList)
            {
                PickupItemUI pickupItemUI = Instantiate(pickupItemUIPerfab, pickupItemUIContainer);
                pickupItemUI.SetItemName(pickup.ItemName);
            }
            
        }
    }
}
