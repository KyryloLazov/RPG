using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Controller;

public class WeaponPickUp : MonoBehaviour, Iraycatable
{
    [SerializeField] Weapon weapon;

    public CursorType GetCursorType()
    {
        return CursorType.PickUp;
    }

    public bool HandleRaycast(PlayerController controller)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pickup(controller.GetComponent<Fighter>());
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Pickup(other.GetComponent<Fighter>());
        }
    }

    private void Pickup(Fighter other)
    {
        other.EquipWeapon(weapon);
        Destroy(gameObject);
    }
}
