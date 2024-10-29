using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace RPG.Core
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] private MMF_Player targetPlayer;

        public void SpawnFloatingText(string textToSpawn)
        {
            MMF_FloatingText floatingText = targetPlayer.GetFeedbackOfType<MMF_FloatingText>();
            floatingText.Value = textToSpawn;
            targetPlayer.PlayFeedbacks(transform.position);   
        }       
        
        public void SpawnFloatingText(int value)
        {
            MMF_FloatingText floatingText = targetPlayer.GetFeedbackOfType<MMF_FloatingText>();
            floatingText.Value = value.ToString();
            targetPlayer.PlayFeedbacks(transform.position);   
        }
    }
}
