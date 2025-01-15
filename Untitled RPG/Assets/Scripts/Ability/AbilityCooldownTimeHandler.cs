using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Ability
{
    public class AbilityCooldownTimeHandler : MonoBehaviour
    {
        private Dictionary<InventoryItem, float> cooldownTimeDict = new();
        
        public event Action<InventoryItem, float> OnCooldownTimeChanged;

        void Update()
        {
            foreach (InventoryItem item in cooldownTimeDict.Keys.ToList())
            {
                cooldownTimeDict[item] -= Time.deltaTime;
                float cooldownTime = cooldownTimeDict[item];
                
                if(cooldownTime <= 0)
                    cooldownTimeDict.Remove(item);
                
                OnCooldownTimeChanged?.Invoke(item, cooldownTime);
            }
        }

        public void StartCooldown(InventoryItem item, float cooldownTime)
        {
            cooldownTimeDict[item] = cooldownTime;   
        }

        public float GetTimeRemainingBeforeUse(InventoryItem item)
        {
            return cooldownTimeDict.GetValueOrDefault(item, 0);
        }
    }
}
