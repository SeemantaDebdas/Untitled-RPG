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

        public event Action<InventoryItemUI> OnItemClicked, OnItemBeginDrag, OnItemDroppedOn, OnItemEndDrag;
        
        public static event Action<InventoryItemUI> 
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
            
            //print("Setting data");
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
            
            print("On Begin Called");
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            print("On End Called");
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            print("On Drop Called");
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData){}
    }
}