using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Shop.UI
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameText = null;
        [SerializeField] private Image itemImage = null;
        [SerializeField] TextMeshProUGUI availablilityText = null;
        [SerializeField] private TextMeshProUGUI priceText = null;
        
        public void Setup(ShopItem shopItem)
        {
            itemNameText.text = shopItem.GetName();
            itemImage.sprite = shopItem.GetIcon();
            availablilityText.text = shopItem.GetAvailability().ToString();
            priceText.text = $"${shopItem.GetPrice():N2}";
        }
    }
}