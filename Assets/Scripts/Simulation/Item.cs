﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemType
{
    Weapon, Drink, Book, Cigarette, Kitchen, Car, Ball, Pants, Tools
}
public class Item : MonoBehaviour
{
    public ItemType type;
    [Header("Storytelling elements")]
    public GameObject itemAdded;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public GameObject itemFall;
    public GameObject itemUse;
    public GameObject itemDeath;
    public float offsetDeadX = 1f;
    public float offsetDeadY = 1f;
    public int maxIterations = 4;
    static float instantiationDelta = 1f;
    bool isUsed = false;
    public bool IsUsed
    {
        get { return isUsed; }
    }
    AudioSource itemUsedSound;
    bool haveSound = false;
    private void Start()
    {
        haveSound = TryGetComponent<AudioSource>(out itemUsedSound);
        if (haveSound)
            itemUsedSound.enabled = false;
    }

    public virtual void OnItemAdded(AIAgent agent)
    {
        agent.items.Add(this);
        isUsed = true;
        if (!itemAdded)
            return;
        if (haveSound)
            itemUsedSound.enabled = true;
        transform.GetChild(0).gameObject.SetActive(false);
        ItemManager.Instance.Positions.Add(transform.position);
        Instantiate(itemAdded, transform.position, Quaternion.identity);
    }

    public void OnItemUse(AIAgent agent, Vector3 position)
    {
        if (!itemUse)
            return;

        ItemManager.Instance.Positions.Add(position);
        Instantiate(itemUse, position, Quaternion.identity);
    }

    public void OnItemUse(AIAgent agent)
    {
        if (!itemUse)
            return;

        ItemManager.Instance.Positions.Add(transform.position);
        Instantiate(itemUse, transform.position, Quaternion.identity);
    }

    // Call when the agent changed its state from calm to alert/attacked
    public void OnStateChanged(AIAgent agent)
    {
        if (!itemFall)
            return;

        Vector2 position = Vector2.zero;
        do
        {
            float offsetXDelta = Random.Range(-instantiationDelta, instantiationDelta);
            float offsetYDelta = Random.Range(-instantiationDelta, instantiationDelta);
            position.x = agent.transform.position.x + offsetXDelta;
            position.y = agent.transform.position.y + offsetYDelta;
            itemFall.transform.position = position;
            Collider collider = itemFall.GetComponent<Collider>();
        } while (ItemManager.Instance.IsValidPosition(position));
        ItemManager.Instance.Positions.Add(position);
        Instantiate(itemFall, new Vector3(position.x, position.y, agent.transform.position.z), Quaternion.identity);
    }
    /** /
    public bool CheckDistance(Vector2 position)
    {
        var objectsClose = lastInstantion.Where(x => Vector2.Distance(position, x) < distance);
        return objectsClose.Count() > 0;
    }
    /**/
    public void OnDeath(AIAgent agent)
    {
        if (!itemDeath)
            return;

        ItemManager.Instance.Positions.Add(agent.transform.position);
        Vector2 position = Vector2.zero;
        do
        {
            float offsetXDelta = Random.Range(-instantiationDelta, instantiationDelta);
            float offsetYDelta = Random.Range(-instantiationDelta, instantiationDelta);
            position.x = agent.transform.position.x + offsetXDelta;
            position.y = agent.transform.position.y + offsetYDelta;
        } while (ItemManager.Instance.IsValidPosition(position));
        ItemManager.Instance.Positions.Add(position);
        Instantiate(itemDeath, new Vector3(position.x, position.y, agent.transform.position.z), Quaternion.identity);
    }
}
