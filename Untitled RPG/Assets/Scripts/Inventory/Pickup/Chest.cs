using System.Collections.Generic;
using RPG.Inventory.Model;
using UnityEngine;
using System.Collections;

namespace RPG.Inventory.Pickup
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] Pickup pickupPrefab;
        [SerializeField] List<InventoryItemSO> pickupItemList;

        public void OpenChest()
        {
            GetComponent<Animator>().Play("Open");
            StartCoroutine(SpawnCoroutine());
        }

        IEnumerator SpawnCoroutine()
        {
            Animator animator = GetComponent<Animator>();

            yield return new WaitUntil(() =>
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName("Open") && stateInfo.normalizedTime > 0.65f;
            });

            SpawnPickups();
        }

        void SpawnPickups()
        {
            int numOfPickups = pickupItemList.Count;
            float angleRange = 180f; // -90° to 90°
            float startAngle = -angleRange / 2f;
            float angleStep = numOfPickups > 1 ? angleRange / (numOfPickups - 1) : 0;

            for (int i = 0; i < numOfPickups; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
                Vector3 force = (direction.normalized + Vector3.up * 2f) * 3f; // tweak force multiplier as needed

                Pickup pickup = Instantiate(pickupPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                pickup.SetItem(pickupItemList[i]);
                pickup.Throw(force);
            }
        }

    }
}
