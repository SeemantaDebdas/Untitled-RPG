using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.Inventory.UI
{
    public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler 
    {
        [SerializeField] Sprite activeSprite, inactiveSprite;
        [SerializeField] Image itemImage, borderImage;
        [SerializeField] TextMeshProUGUI quantityText;

        public event Action<InventoryItemUI> OnItemClicked,
            OnItemDroppedOn,
            OnItemBeginDrag,
            OnItemEndDrag,
            OnRightMouseButtonClick;

        private bool IsEmpty => !itemImage.gameObject.activeSelf;

        public void Initialize()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            quantityText.text = "";
            print("Resetting data");
        }

        public void Select()
        {
            borderImage.sprite = activeSprite;
        }

        public void Deselect()
        {
            borderImage.sprite = inactiveSprite;
        }

        public void SetData(Sprite itemSprite, int quantity)
        {
            itemImage.sprite = itemSprite;
            quantityText.text = quantity.ToString();
            itemImage.gameObject.SetActive(true);
            
            print("Setting data");
        }
        
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
            else
            {
                OnRightMouseButtonClick?.Invoke(this);
            }          
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsEmpty) return;
            
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData){}
    }
}