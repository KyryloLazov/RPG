using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PRG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        [SerializeField] Text healthText;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            Health target = fighter.GetTarget();
            if(target != null)
            {
                healthText.text = String.Format("{0:0}/{1:0}", target.getHealth(), target.getMaxHealth());
            }
            else
            {
                healthText.text = "N/A";
            }
            
        }
    }
}