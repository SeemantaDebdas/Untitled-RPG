using MoreMountains.Tools;
using RPG.Core;
using UnityEngine;

namespace RPG.UI
{
    public class ManaController : MonoBehaviour
    {
        [SerializeField] private MMProgressBar manaView = null;
        [SerializeField] private Mana manaData = null;

        void OnEnable()
        {
            manaData.OnManaChanged += UpdateView;
        }

        void OnDestroy()
        {
            manaData.OnManaChanged -= UpdateView;
        }

        void UpdateView()
        {
            manaView.UpdateBar01((float)manaData.CurrentMana / manaData.MaxMana);    
        }
    }
}
