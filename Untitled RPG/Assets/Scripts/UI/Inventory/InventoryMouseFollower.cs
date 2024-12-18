using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Inventory.UI
{
    public class InventoryMouseFollower : MonoBehaviour
    {
        Canvas rootCanvas;
        InventoryItemUI inventoryItemUI;
        RectTransform rectTransform;

        private UnityEngine.Camera mainCamera;

        private void Awake()
        {
            rootCanvas = transform.root.GetComponent<Canvas>();
            mainCamera = UnityEngine.Camera.main;
            inventoryItemUI = GetComponentInChildren<InventoryItemUI>();
            
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectTransform.position = Mouse.current.position.ReadValue(); // change this to use not just mouse but gamepad too
        }
        
        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);
        
        public void SetData(Sprite itemSprite, int quantity)
        {
            inventoryItemUI.SetData(itemSprite, quantity);
        }
    }
}
