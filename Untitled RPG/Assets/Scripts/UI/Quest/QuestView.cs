using RPG.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Quest.UI
{
    public class QuestView : View
    {        
        [SerializeField] private InputActionReference enableAction, disableAction;

        public override void Initialize()
        {
            enableAction.action.performed += _ =>
            {
                OnShowRequest?.Invoke();
            };

            disableAction.action.performed += _ =>
            {
                OnHideRequest?.Invoke();
            };
        }
        
    }
}
