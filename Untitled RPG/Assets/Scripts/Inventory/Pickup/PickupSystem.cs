using System;
using System.Collections.Generic;
using RPG.Inventory.Model;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Inventory.Pickup
{
    public class PickupSystem : MonoBehaviour
    {
        [SerializeField] private InputActionReference pickupAction, scrollAction = null;
        [SerializeField] private GridInventorySO gridInventory;
        [SerializeField] private List<Pickup> pickupList = new();

        public int SelectedItemIndex { get; private set; } = 0;
        public event Action<List<Pickup>> OnPickupListChanged;
        public event Action<int> OnSelectedItemChanged;

        private void OnEnable()
        {
            pickupAction.action.performed += _ =>
            {
                if (pickupList.Count <= 0) return;

                Pickup pickup = pickupList[SelectedItemIndex];
                gridInventory.AddItem(pickup.Item, pickup.Quantity);

                HandlePickupRemovalFromList(pickup, SelectedItemIndex);

                Destroy(pickup.gameObject);
            };

            scrollAction.action.performed += context =>
            {
                float input = 0;

                if (context.control.device is Mouse)
                {
                    input = context.ReadValue<Vector2>().y * -1;
                }
                else if (context.control.device is Gamepad)
                {
                    input = context.ReadValue<float>();
                }

                UpdateSelectedItemIndexOnScroll(input);
            };

            pickupAction.action.Enable();
            scrollAction.action.Enable();
        }

        private void UpdateSelectedItemIndexOnScroll(float input)
        {
            if (pickupList.Count <= 0)
                return;

            SelectedItemIndex += (int)input;

            if (SelectedItemIndex >= pickupList.Count)
                SelectedItemIndex = 0;
            else if (SelectedItemIndex < 0)
                SelectedItemIndex = pickupList.Count - 1;

            OnSelectedItemChanged?.Invoke(SelectedItemIndex);
        }

        private void OnDisable()
        {
            pickupAction.action.Disable();
            scrollAction.action.Disable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Pickup pickup))
                return;

            if (pickupList.Contains(pickup))
            {
                //print("Already contains pickup");
                return;
            }

            //print(other.name);
            pickupList.Add(pickup);

            OnPickupListChanged?.Invoke(pickupList);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Pickup pickup))
                return;

            int removeIndex = pickupList.IndexOf(pickup);

            HandlePickupRemovalFromList(pickup, removeIndex);
        }

        private void HandlePickupRemovalFromList(Pickup pickup, int removeIndex)
        {
            pickupList.Remove(pickup);

            //slide the selectedindex to left if item removed from left
            if (pickupList.Count == 0)
            {
                SelectedItemIndex = 0;
                OnPickupListChanged?.Invoke(pickupList);
                OnSelectedItemChanged?.Invoke(SelectedItemIndex);
                return;
            }

            if (removeIndex < SelectedItemIndex)
                SelectedItemIndex--;
            else if (removeIndex == SelectedItemIndex)
                SelectedItemIndex = Mathf.Min(SelectedItemIndex, pickupList.Count - 1);

            OnPickupListChanged?.Invoke(pickupList);
            OnSelectedItemChanged?.Invoke(SelectedItemIndex);
        }
    }
}
