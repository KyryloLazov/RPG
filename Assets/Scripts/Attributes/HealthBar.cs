using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PRG.Attributes;

namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour
    {

        [SerializeField] Health health;
        [SerializeField] RectTransform foreground;
        [SerializeField] Canvas canvas;

        void Update()
        {
            foreground.localScale = new Vector3(health.GetPercentage(), 1, 1);
            if(Mathf.Approximately(health.getHealth(), 0))
            {
                canvas.enabled = false;
            }

            if (Mathf.Approximately(health.getHealth(), health.getMaxHealth()))
            {
                canvas.enabled = false;
            }

            else
            {
                canvas.enabled = true;
            }
        }
    }
}