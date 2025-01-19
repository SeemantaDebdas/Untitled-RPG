using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class CharacterEquipment : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer bodyRenderer, legsRenderer = null;
        float defaultBodyWeight, defaultArmWeight, defaultLegsWeight;

        private void Awake()
        {
            defaultBodyWeight = bodyRenderer.GetBlendShapeWeight(0);
            defaultArmWeight = bodyRenderer.GetBlendShapeWeight(1);
            defaultLegsWeight = legsRenderer.GetBlendShapeWeight(0);
        }

        public void SetBodyWeight(float bodyWeight)
        {
            bodyRenderer.SetBlendShapeWeight(0, bodyWeight);
        }

        public void SetArmWeight(float armWeight)
        {
            bodyRenderer.SetBlendShapeWeight(1, armWeight);
        }

        public void SetLegsWeight(float legsWeight)
        {
            legsRenderer.SetBlendShapeWeight(0, legsWeight);
        }

        public void ResetBodyWeight()
        {
            bodyRenderer.SetBlendShapeWeight(0, defaultBodyWeight);
        }

        public void ResetArmWeight()
        {
            bodyRenderer.SetBlendShapeWeight(1, defaultArmWeight);
        }
        
        public void ResetLegsWeight()
        {
            legsRenderer.SetBlendShapeWeight(0, defaultLegsWeight);
        }
    }
}
