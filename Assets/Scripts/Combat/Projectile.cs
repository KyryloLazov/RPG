using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PRG.Attributes;
using UnityEngine.Events;
using Unity.VisualScripting;

public class Projectile : MonoBehaviour
{  
    [SerializeField] float speed = 3f;
    [SerializeField] bool isHoming;
    [SerializeField] GameObject HitEffect  = null;
    [SerializeField] UnityEvent Hit;
    [SerializeField] float MaxLifeTime = 10f;
    //[SerializeField] GameObject[] DestroyOnHit; 

    GameObject instigator = null;
    Health target;
    float damage = 0f;
    void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        if(isHoming && !target.isDead)
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;

        Destroy(gameObject, MaxLifeTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
        if (collider == null) return target.transform.position;
        return target.transform.position + Vector3.up * (collider.height / 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
       
        if (target.isDead) return; 
        target.TakeDamage(instigator, damage);
        Hit.Invoke();

        if (HitEffect != null) 
        { 
            Instantiate(HitEffect, GetAimLocation(), transform.rotation); 
        }
        
        //foreach(GameObject toDestroy in DestroyOnHit)
        //{
        //    Destroy(toDestroy);
        //}

        Destroy(gameObject);
    }
}
