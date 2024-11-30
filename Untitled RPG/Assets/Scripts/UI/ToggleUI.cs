using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.UI
{
    public class ToggleUI : MonoBehaviour
    {
        [SerializeField] private InputActionReference enableAction, disableAction;
        
        private void OnEnable()
        {
            enableAction.action.performed += _ =>
            {
                gameObject.SetActive(true);
            };

            disableAction.action.performed += _ =>
            {
                gameObject.SetActive(false);
            };
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
