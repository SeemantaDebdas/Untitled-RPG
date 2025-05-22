using TMPro;
using UnityEngine;

namespace RPG.Inventory.Pickup.UI
{
    public class PickupItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName = null;
        
        public void SetItemName(string pickupItemName) => itemName.text = pickupItemName;
    }
}
