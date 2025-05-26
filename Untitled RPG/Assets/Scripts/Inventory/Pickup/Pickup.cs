using System;
using System.Collections;
using RPG.Core;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Inventory.Pickup
{
    public class Pickup : MonoBehaviour
    {
        [field: SerializeField] public InventoryItemSO Item { get; private set; } = null;
        [field: SerializeField] public int Quantity { get; private set; } = 1;
        [field: SerializeField] LayerMask environmentLayer;

        public string ItemName => Item.DisplayName;

        public void SetItem(InventoryItemSO inventoryItemSO)
        {
            Item = inventoryItemSO;
        }

        public void Throw(Vector3 force)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(force, ForceMode.Impulse);

            StartCoroutine(MoveCoroutine());
        }

        private IEnumerator MoveCoroutine()
        {

            yield return new WaitUntil(() => IsNearGround());
            //print("Near Ground");
            Destroy(GetComponent<Rigidbody>());
        }

        private bool IsNearGround()
        {
            if (Physics.Raycast(transform.position, Vector3.down, 0.1f, environmentLayer))
                return true;

            return false;
        }
    }
}
