using System;
using UnityEngine;

namespace RPG.UI
{
    public abstract class View : MonoBehaviour
    {
        public abstract void Initialize();
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Show() => gameObject.SetActive(true);
        public Action OnShowRequest, OnHideRequest;
    }
}