using TMPro;
using UnityEngine;

namespace RPG.Inventory.Pickup.UI
{
    public class PickupItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName = null;
        [SerializeField] private GameObject selectedBorder = null;

        public void SetItemName(string pickupItemName) => itemName.text = pickupItemName;

        public void SetSelectedBorder(bool selected)
        {
            selectedBorder.SetActive(selected);
            // if(selected)
            //     print(itemName.text + " is selected");
        }
    }
}
