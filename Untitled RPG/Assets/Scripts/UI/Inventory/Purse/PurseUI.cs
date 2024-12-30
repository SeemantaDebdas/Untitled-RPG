using System;
using RPG.Inventory.Model;
using TMPro;
using UnityEngine;

namespace RPG.Inventory.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] private CoinPurse coinPurse = null;
        [SerializeField] private TextMeshProUGUI balanceTextField = null;

        private void OnEnable()
        {
            coinPurse.OnBalanceUpdated += RefreshUI;
        }

        private void OnDestroy()
        {
            coinPurse.OnBalanceUpdated -= RefreshUI;
        }

        private void Start()
        {
            RefreshUI();
        }

        void RefreshUI()
        {
            balanceTextField.text = $"{coinPurse.GetBalance():N2}";
        }
    }
}
