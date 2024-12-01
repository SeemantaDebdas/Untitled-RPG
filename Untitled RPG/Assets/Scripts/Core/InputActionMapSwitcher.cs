using System;
using RPG.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RPG.Core
{
    public class InputActionMapSwitcher : MonoBehaviour
    {
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
            ViewManager.OnAnyViewShow += _ =>
            {
                playerInput.SwitchCurrentActionMap(uiActionMap.name);
            };

            ViewManager.OnDefaultViewShow += _ =>
            {
                playerInput.SwitchCurrentActionMap(playerActionMap.name);
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
            //Debug.Log($"Cancel Action Enabled: {uiCancelAction.action.enabled}");
        }
        
    }
}
