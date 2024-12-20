using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] FieldOfView fieldOfView;
        [SerializeField] InputActionReference interactionAction;

        List<IInteractable> focusList = new(), inRangeList = new();

        private void Awake()
        {
            if (fieldOfView == null)
            {
                fieldOfView = GetComponent<FieldOfView>();
            }
        }

        private void OnEnable()
        {
            interactionAction.action.performed += context =>
            {
                //Get closest in focus list.
                if (focusList.Count > 0)
                {
                    focusList[0].Interact(this);
                }
            };
            interactionAction.action.Enable();
        }

        public bool HasInteractable()
        {
            return focusList.Count > 0;
        }

        private void Update()
        {
            List<Collider> objectsInRange = fieldOfView.ObjectsInRange;
            
            focusList.RemoveAll(interactable => !interactable.IsAlive());
            inRangeList.RemoveAll(interactable => !interactable.IsAlive());
            objectsInRange.RemoveAll(objectInRange => objectInRange == null);
            
            CallInRangeOnInteractables(objectsInRange);

            List<Transform> objectsInSight = fieldOfView.GetValidTargets();
            objectsInSight = CheckIfCameraSeesObjects(objectsInSight);
            
            CallFocusOnInteractables(objectsInSight);
        }


        private void CallInRangeOnInteractables(List<Collider> objectsInRange)
        {
            ClearInteractablesNotInRange(objectsInRange);
            
            if (objectsInRange.Count == 0)
                return;

            foreach (Collider objectInRange in objectsInRange)
            {
                if (objectInRange == null)
                    continue;
                
                if (!objectInRange.TryGetComponent(out IInteractable interactable))
                    continue;
                
                if(!interactable.IsAlive())
                    continue;

                if (inRangeList.Contains(interactable))
                    continue;

                interactable.InRange(this);
                inRangeList.Add(interactable);
            }
        }

        private void ClearInteractablesNotInRange(List<Collider> objectsInRange)
        {
            foreach (IInteractable interactable in inRangeList.ToList())
            {
                // Check if the interactable or its GameObject is null (destroyed)
                if (!interactable.IsAlive())
                {
                    // Handle destroyed object here
                    inRangeList.Remove(interactable);
                    Debug.LogWarning("Interactable or its GameObject is destroyed.");
                    continue;
                }
                
                Collider interactableCollider = interactable.gameObject.GetComponent<Collider>();

                if (objectsInRange.Contains(interactableCollider))
                    continue;

                interactable.OutOfRange(this);
                inRangeList.Remove(interactable);
            }
        }

        private void CallFocusOnInteractables(List<Transform> objectsInSight)
        {
            ClearInteractablesNotInFocus(objectsInSight);
            
            if (objectsInSight.Count == 0)
                return;

            foreach (Transform objectInSight in objectsInSight)
            {
                if (objectInSight == null)
                    continue;
                
                if (!objectInSight.TryGetComponent(out IInteractable interactable))
                    continue;

                if (focusList.Contains(interactable))
                    continue;
                
                interactable.Focus(this);
                focusList.Add(interactable);
            }
        }

        private void ClearInteractablesNotInFocus(List<Transform> objectsInSight)
        {
            foreach (IInteractable interactable in focusList.ToList())
            {
                // Check if the interactable or its Transform is null (destroyed)
                if (!interactable.IsAlive())
                {
                    // Handle destroyed object here
                    focusList.Remove(interactable);
                    Debug.LogWarning("Interactable or its GameObject is destroyed.");
                    continue;
                }
                
                Transform interactableTransform = interactable.transform;
                if (!objectsInSight.Contains(interactableTransform))
                {
                    interactable.UnFocus(this);
                    focusList.Remove(interactable);
                }
            }
        }


        List<Transform> CheckIfCameraSeesObjects(List<Transform> objectsInSight)
        {
            List<Transform> toRemove = new();

            foreach (Transform objectInSight in objectsInSight)
            {
                if (objectInSight == null)
                    continue;
                
                float centerX = 0.5f;
                float centerXOffset = 0.1f;

                float objectOnScreenX = UnityEngine.Camera.main.WorldToViewportPoint(objectInSight.position).x;
                
                if(objectOnScreenX > centerX + centerXOffset || objectOnScreenX < centerX - centerXOffset)
                    toRemove.Add(objectInSight);
            }
            
            foreach (Transform objectInSight in toRemove)
            {
                objectsInSight.Remove(objectInSight);
            }
            
            return objectsInSight;
        }
    }
}
