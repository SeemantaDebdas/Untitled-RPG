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
        [SerializeField] private PickupItemUI pickupItemUIPerfab = null;

        List<PickupItemUI> pickupItemUIList = new List<PickupItemUI>();

        private void Start()
        {
            pickupSystem.OnPickupListChanged += PickupSystem_OnPickupListChanged;
            pickupSystem.OnSelectedItemChanged += PickupSystem_OnSelectedItemChanged;
            backgroundImage.enabled = false;
        }

        private void OnDestroy()
        {
            pickupSystem.OnPickupListChanged -= PickupSystem_OnPickupListChanged;
            pickupSystem.OnSelectedItemChanged -= PickupSystem_OnSelectedItemChanged;
        }

        private void PickupSystem_OnPickupListChanged(List<Pickup> pickups)
        {
            for (int i = 0; i < pickupItemUIContainer.childCount; i++)
            {
                Destroy(pickupItemUIContainer.GetChild(i).gameObject);
            }
            pickupItemUIList.Clear();

            for (int i = 0; i < pickups.Count; i++)
            {
                PickupItemUI pickupItemUI = Instantiate(pickupItemUIPerfab, pickupItemUIContainer);
                pickupItemUI.SetItemName(pickups[i].ItemName);
                pickupItemUIList.Add(pickupItemUI);
            }

            for (int i = 0; i < pickupItemUIList.Count; i++)
            {
                pickupItemUIList[i].SetSelectedBorder(i == pickupSystem.SelectedItemIndex);
            }
        }

        private void PickupSystem_OnSelectedItemChanged(int selectedItemIndex)
        {
            for (int i = 0; i < pickupItemUIList.Count; i++)
            {
                pickupItemUIList[i].SetSelectedBorder(i == selectedItemIndex);
            }
        }
    }
}
