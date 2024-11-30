using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RPG.Core
{
    public class InputActionMapSwitcher : MonoBehaviour
    {
        [SerializeField] InputActionReference switchAction;
        [SerializeField] InputActionReference uiCancelAction;

        PlayerInput playerInput;

        private InputActionMap playerActionMap, uiActionMap;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            var actions = playerInput.actions;
            playerActionMap = actions.FindActionMap("Player");
            uiActionMap = actions.FindActionMap("UI");
        }

        private void Start()
        {
            switchAction.action.performed += _ =>
            {
                SwitchActionMap();
            };

            uiCancelAction.action.performed += _ =>
            {
                SwitchActionMap();
            };
        }

        void SwitchActionMap()
        {
            if (playerActionMap.enabled)
            {
                Debug.Log("Switching to UI Action Map");
                playerInput.SwitchCurrentActionMap(uiActionMap.name);
            }
            else
            {
                Debug.Log("Switching to Player Action Map");
                playerInput.SwitchCurrentActionMap(playerActionMap.name);
            }

            Debug.Log($"Current Action Map: {playerInput.currentActionMap.name}");
            Debug.Log($"UI Action Map Enabled: {uiActionMap.enabled}");
            Debug.Log($"Cancel Action Enabled: {uiCancelAction.action.enabled}");
        }

        private void Update()
        {
        }
    }
}
