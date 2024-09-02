using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DeleteText : MonoBehaviour
    {
        [SerializeField] GameObject target = null;
        public void Destroy()
        {
            Destroy(target);
        }
    }
}