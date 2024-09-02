using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.ScenesManager
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup group;
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
            while (group.alpha < 1)
            {
                group.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (group.alpha > 0)
            {
                group.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}