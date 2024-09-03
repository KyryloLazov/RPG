using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PRG.Attributes
{
    public class ExpierenceDisplay : MonoBehaviour
    {
        Expierence exp;
        [SerializeField] Text XPText;

        private void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<Expierence>();
        }

        // Update is called once per frame
        void Update()
        {
            XPText.text = "" + exp.GetXP();
        }
    }
}