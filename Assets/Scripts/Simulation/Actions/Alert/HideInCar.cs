using System.Collections;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Actions/HideInCar")]
public class HideInCar : Action
{
    public float distanceToHide = 2f;
    public float distanceToAdd = 2f;
    [System.NonSerialized] Car car;
    [System.NonSerialized] Transform targetTrf;
    [System.NonSerialized] InfectedAgent infected;
    [System.NonSerialized] Weapon weapon = null;
    [System.NonSerialized] Transform weaponLocation;
    [System.NonSerialized] bool fisrtTime = true;
    [System.NonSerialized] bool weaponFound = true;
    [System.NonSerialized] bool haveWeapon = false;
    public override void Execute(AIAgent agent)
    {
        if (!car)
            return;

        if(!car.IsUsed)
        {
            agent.Target = targetTrf;
            if (Vector2.Distance(targetTrf.position, agent.transform.position) < distanceToHide)
            { 
                car.Hide(agent);
            }
        }
        else if(car.IsBroken && !haveWeapon)
        {
            if (fisrtTime)
            {
                weaponFound = FoundWeapon(agent);
                fisrtTime = false;
            }
                
            if(weaponFound)
            {
                agent.Target = weaponLocation;
                if(Vector2.Distance(agent.transform.position, weaponLocation.position) <= distanceToAdd)
                {
                    weapon.OnItemAdded(agent);
                    haveWeapon = true;
                }
                    
            }  
        }
        if (haveWeapon)
            agent.Target = infected.transform;

    }

    public bool FoundWeapon(AIAgent agent)
    {
        var weapons = FindObjectsOfType<Weapon>().Where(x => x.IsFree && x.environment == agent.Environment);
        float minDistance = Mathf.Infinity;
        AttackType attackType = (AttackType)Random.Range(1, 2);
        foreach (var currWeapon in weapons)
        {

            float distance = Vector2.Distance(agent.transform.position, currWeapon.transform.position);
            if (distance < minDistance)
            {
                this.weapon = currWeapon;
                weaponLocation = currWeapon.transform;
                minDistance = distance;

            }


        }
        return weapon != null ? true : false;
    }

    public override bool IsComplete(AIAgent agent)
    {
        return !car;
    }

    public override void OnActionFinish(AIAgent agent)
    {
       
    }

    public override void OnActionPrepare(AIAgent agent)
    {
        car = FindObjectOfType<Car>();
        if (!car.Reserved)
        {
            infected = StoryManager.Instance.Infected;
            targetTrf = car.doorEntry;
            car.Reserved = true;
        }
        else
        {
            car = null;
            block = true;
        }
            
    }
}
