using System;
using DG.Tweening;
using RPG.Core;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Temporary Set Variable Effect", menuName = "Debdas/Abilities/Effect/Temporary Set Variable Effect")]
    public class TemporarySetVariableEffect : EffectStrategy, IValueSetter<float>
    {
        [SerializeField] private ScriptableFloat variable;
        [SerializeField] private float newValue = 5f;
        
        private float defaultValue;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            Debug.Log(newValue);
            defaultValue = variable.Value;
            SetValue(newValue);
            DOVirtual.Float(0, 1, data.GetAbilityDuration(), _ => { }).OnComplete(() =>
            {
                Debug.Log($"Resetting Temporary Set Variable Effect");
                SetValue(defaultValue);
                onFinished?.Invoke();
            });
        }

        public void SetValue(float value)
        {
            variable.SetValue(value, this);
        }
    }
}
