using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class SpawnText : MonoBehaviour
    {
        [SerializeField] DamageText DamageText;
        void Start()
        {
        }

        public void Spawn(float damage)
        {
            DamageText instance = Instantiate<DamageText>(DamageText, transform);
            instance.SetText(damage);
        }
    }
}