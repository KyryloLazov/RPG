using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.ScenesManager
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup group;
        Coroutine currentActiveFade = null;
        private void Awake()
        {              
            group = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            group.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        public IEnumerator Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeCoroutine(target, time));
            yield return currentActiveFade;
        }

        private IEnumerator FadeCoroutine(float target, float time)
        {
            while (!Mathf.Approximately(group.alpha, target))
            {
                group.alpha = Mathf.MoveTowards(group.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

       
    }
}