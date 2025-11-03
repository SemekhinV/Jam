using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class Hider : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.5f;
    
        public void HideAndShow(Action onHideAction, Action onComplete = null)
        {
            StartCoroutine(HideAndShowRoutine(onHideAction, onComplete));
        }
    
        private IEnumerator HideAndShowRoutine(Action onHideAction, Action onComplete)
        {
            // Затемнение экрана
            yield return StartCoroutine(FadeCanvasGroup(0f, 1f, _fadeDuration));
        
            // Вызываем действие когда экран полностью затемнен
            onHideAction?.Invoke();
        
            // Осветление экрана
            yield return StartCoroutine(FadeCanvasGroup(1f, 0f, _fadeDuration));

            onComplete?.Invoke();
        }
    
        private IEnumerator FadeCanvasGroup(float fromAlpha, float toAlpha, float duration)
        {
            float elapsedTime = 0f;
        
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / duration);
                _canvasGroup.alpha = currentAlpha;
                yield return null;
            }
        
            // Убеждаемся, что достигли целевого значения
            _canvasGroup.alpha = toAlpha;
        }
    }
}
