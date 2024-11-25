using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.UI
{
    public class UISizeAnimator : MonoBehaviour
    {
        [SerializeField] private bool shouldAnimate = false;
        [SerializeField] private bool minimizedAtStart = true;
        [SerializeField] private Vector3 targetSize;
        [SerializeField] float minimizeDuration, maximizeDuration;
        [SerializeField] Ease easeType = Ease.Linear;

        [Space] 
        [SerializeField] UnityEvent OnMinimized;
        [SerializeField] UnityEvent OnMaximized;
        
        Vector3 initialScale;

        private void Awake()
        {
            initialScale = transform.localScale;
            
            if(minimizedAtStart)
                Minimize();
        }

        public void Maximize()
        {
            DOTween.Kill(transform);
            
            transform.DOScale(initialScale, maximizeDuration).SetEase(easeType).OnComplete(() =>
            {
                if(shouldAnimate)
                    transform.DOScale(targetSize, maximizeDuration).SetLoops(-1, LoopType.Yoyo).SetEase(easeType);
                
                OnMaximized.Invoke();
            });
        }

        public void Minimize()
        {
            DOTween.Kill(transform);
            transform.DOScale(Vector3.zero, minimizeDuration).SetEase(easeType).OnComplete(() => OnMinimized?.Invoke());
        }
    }
}
