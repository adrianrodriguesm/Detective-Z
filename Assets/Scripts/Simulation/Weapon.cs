using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public AttackType attackType;
    public float damage;
    public float radiusAttack;
    [HideInInspector]
    public bool isFree = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Agent"))
        {
            collision.GetComponent<AIAgent>().items.Add(this);
            isFree = false;
            GetComponent<Collider2D>().enabled = false;
            this.transform.parent = collision.transform;
        }
           
    }
}
