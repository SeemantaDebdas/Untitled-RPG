using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] List<View> viewList = new();
        [SerializeField] View defaultView = null;

        View currentView = null;

        public static event Action<View> OnAnyViewShow, OnDefaultViewShow;
        void Awake()
        {
            foreach (var view in viewList)
            {
                view.Initialize();
                view.Hide();
                
                view.OnShowRequest += () =>
                {
                    SwitchView(view);
                    OnAnyViewShow?.Invoke(view);
                };
                
                view.OnHideRequest += () =>
                {
                    SwitchView(defaultView);
                    OnDefaultViewShow?.Invoke(defaultView);
                };
            }

            currentView = defaultView;
            currentView.Show();
        }

        private void OnEnable()
        {
            SwitchView(defaultView);
        }

        public void SwitchView(View view)
        {
            if (currentView == null || currentView == view)
                return;

            Debug.Log(view.name);

            currentView.Hide(); 
            currentView = view;
            currentView.Show();
        }
    }
}