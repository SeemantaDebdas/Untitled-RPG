using System;
using TMPro;
using UnityEngine;

namespace RPG.Inventory.UI
{
    public class InventoryDescriptionUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText, descriptionText;
        private void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            titleText.text = "";
            descriptionText.text = "";
            gameObject.SetActive(false);
        }

        public void SetDescription(string title, string description)
        {
            titleText.text = title;
            descriptionText.text = description;
            gameObject.SetActive(true);
        }
    }
}
