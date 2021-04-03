using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public AttackType attackType;
    public float damage;
    public float radiusAttack;

    bool isFree = true;
    public bool IsFree
    {
        get { return isFree; }
        set { isFree = value; }
    }
    private void Start()
    {
        type = ItemType.Weapon;
    }

    public override void OnItemAdded(AIAgent agent)
    {
        base.OnItemAdded(agent);
        Debug.Log("Added!");
        isFree = false;
        this.transform.parent = agent.transform;
        //GetComponent<Collider2D>().enabled = false;
    }
    /** /
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
    /**/
}
