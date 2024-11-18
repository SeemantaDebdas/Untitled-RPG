using MoreMountains.Tools;
using RPG.Core;
using UnityEngine;

namespace RPG.UI
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private MMProgressBar healthView;
        [SerializeField] private Health healthModel;

        private void OnEnable()
        {
            healthModel.OnDamage += (data => UpdateView());
            healthModel.OnDeath += (data =>
                    {
                        gameObject.SetActive(false);
                    }
                    );
        }

        private void Start()
        {
            UpdateView();
        }

        void UpdateView()
        {
            healthView.UpdateBar01(healthModel.CurrentHealth / healthModel.MaxHealth);
        }
    }
}
