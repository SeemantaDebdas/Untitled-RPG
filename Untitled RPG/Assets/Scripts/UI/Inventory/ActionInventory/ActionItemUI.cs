using UnityEngine;
using UnityEngine.UI;

namespace RPG.Inventory.UI
{
    public class ActionItemUI : InventoryItemUI
    {
        [SerializeField] private Image cooldownImage = null;

        public override void ResetData()
        {
            base.ResetData();
            cooldownImage.fillAmount = 0f;
        }

        public void SetCooldownImageFillAmount(float amount)
        {
            cooldownImage.fillAmount = amount;
        }
    }
}
