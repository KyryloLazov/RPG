using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Controller;
using PRG.Attributes;

public class WeaponPickUp : MonoBehaviour, Iraycatable
{
    [SerializeField] WeaponConfig weapon;
    [SerializeField] float healthToRestore = 0;
    [SerializeField] float respawnTime = 5f;

    public CursorType GetCursorType()
    {
        return CursorType.PickUp;
    }

    public bool HandleRaycast(PlayerController controller)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pickup(controller.gameObject);
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Pickup(other.gameObject);
        }
    }

    private void Pickup(GameObject subject)
    {
        if(weapon != null)
        {
            subject.GetComponent<Fighter>().EquipWeapon(weapon);
        }
        if(healthToRestore > 0)
        {
            subject.GetComponent<Health>().Heal(healthToRestore);
        }
        StartCoroutine(HideForSeconds(respawnTime));
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    }

    private void ShowPickup(bool shouldShow)
    {
        gameObject.GetComponent<SphereCollider>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }
}
