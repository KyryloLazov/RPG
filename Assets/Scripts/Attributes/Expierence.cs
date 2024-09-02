using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Attributes {

    public class Expierence : MonoBehaviour, ISaveable
    {
        [SerializeField] float expPoints = 0;

        public event Action onXPGained;

        public void GainExp(float exp)
        {
            expPoints += exp;
            onXPGained();
        }

        public float GetXP()
        {
            return expPoints;
        }

        public object CaptureState()
        {
            return expPoints;
        }

        public void RestoreState(object state)
        {
            expPoints = (float)state;
        }
    }
}