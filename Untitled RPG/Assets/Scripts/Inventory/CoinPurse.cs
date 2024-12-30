using System;
using UnityEngine;

namespace RPG.Inventory.Model
{
    public class CoinPurse : MonoBehaviour
    {
        [SerializeField] private float startingBalance = 400f;

        public event Action OnBalanceUpdated;
        float balance = 0f;
        
        private void Awake()
        {
            balance = startingBalance;
        }

        public float GetBalance() => balance;

        public void CreditBalance(float amount)
        {
            balance += amount;
            
            OnBalanceUpdated?.Invoke();
        }

        public void DebitBalance(float amount)
        {
            balance -= amount;
            
            OnBalanceUpdated?.Invoke();
        }
    }
}
