using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PRG.Attributes
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats exp;
        [SerializeField] Text levelText;

        private void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        // Update is called once per frame
        void Update()
        {
            levelText.text = "" + exp.GetLevel();
        }
    }
}