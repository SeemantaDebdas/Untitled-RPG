using MoreMountains.Tools;
using RPG.Core;
using UnityEngine;

namespace RPG.UI
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private MMProgressBar healthView;
        [SerializeField] private Health healthModel;

        private void OnEnable()
        {
            healthModel.OnDamage += _ => UpdateView();
            healthModel.OnDeath += _ =>
            {
                gameObject.SetActive(false);
            };
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
