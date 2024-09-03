using PRG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Controller;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, Iraycatable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController controller)
        {
            Fighter fighter = controller.GetComponent<Fighter>();

            if (!fighter.CanAtack(gameObject))
            {
                return false;
            }
            if ((Input.GetMouseButton(0)))
            {
                fighter.SetTarget(gameObject);
            }
            return true;
        }
            
    }
}
