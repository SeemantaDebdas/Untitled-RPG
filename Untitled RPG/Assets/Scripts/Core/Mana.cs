using System;
using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class Mana : MonoBehaviour
    {
        [field: SerializeField] public int MaxMana { get; private set; } = 100;

        public int CurrentMana { get; private set; } = 0;

        public event Action OnManaChanged;

        [SerializeField] private float regenDelay = 2f; 
        [SerializeField] private float regenRate = 5f;  

        Coroutine regenCoroutine = null;

        private void Start()
        {
            CurrentMana = MaxMana;
            OnManaChanged?.Invoke();
        }

        public void UseMana(int amount)
        {
            if (amount > CurrentMana)
            {
                Debug.LogError("Not enough Mana!");
                return;
            }

            CurrentMana -= amount;
            OnManaChanged?.Invoke();
            
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }

            regenCoroutine = StartCoroutine(RegenerateMana());
        }

        private IEnumerator RegenerateMana()
        {
            // Wait for the regeneration delay
            yield return new WaitForSeconds(regenDelay);

            float manaToAdd = 0; // Accumulator for fractional mana

            // Regenerate mana over time
            while (CurrentMana < MaxMana)
            {
                manaToAdd += regenRate * Time.deltaTime; // Accumulate fractional mana
                int manaPoints = Mathf.FloorToInt(manaToAdd); // Extract integer mana points

                if (manaPoints > 0)
                {
                    CurrentMana += manaPoints;
                    manaToAdd -= manaPoints; // Remove used points from the accumulator
                    CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana); // Ensure we don't exceed MaxMana
                    OnManaChanged?.Invoke();
                }

                yield return null; // Wait for the next frame
            }

            // Stop tracking the coroutine once mana is fully regenerated
            regenCoroutine = null;
        }

    }
}