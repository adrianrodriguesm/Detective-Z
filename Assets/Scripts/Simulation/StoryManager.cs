using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoryManager : Singleton<StoryManager>
{
    List<AIAgent> agents;
    List<AIAgent> deadAgents;
    [HideInInspector]
    public List<AIAgent> AIAgents
    {
        get { return agents; }
    }
    InfectedAgent infected;
    public InfectedAgent Infected
    {
        get { return infected;  }
        set { infected = value; }
    }

    public float timerToStartTheAttack = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        agents = FindObjectsOfType<AIAgent>().ToList();
        deadAgents = new List<AIAgent>();
        infected = FindObjectOfType<InfectedAgent>();
        StartCoroutine(PrepareInfectedAttack());
    }

    IEnumerator PrepareInfectedAttack()
    {
        infected.gameObject.SetActive(false);
        yield return new WaitForSeconds(timerToStartTheAttack);
        infected.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        foreach (var agent in agents.ToList())
        {
           if(agent.IsDead())
           {
               agents.Remove(agent);
               deadAgents.Add(agent);
           }
        }
        /**/
        if (agents.Count == 0 && infected.IsDeadOrEscaped())
        {
            // Generated sprite with scars due the different types of attacks
            infected.enabled = false;
        }
        /**/
           
    }
    public bool AllAgentAreDead()
    {
        return agents.Count == 0;
    }
    public bool IsSimulationEnd()
    {
        return agents.Count == 0 && infected.IsDeadOrEscaped();
    }

    public AIAgent GetAgentToSeek()
    {
        // Find an agent with the highest detection level
        AIAgent targetAgent = null;
        float currDetectionLevel = -1;
        foreach (var agent in agents)
        {
            if (currDetectionLevel <= agent.DetectionLevel)
            {
                currDetectionLevel = agent.DetectionLevel;
                targetAgent = agent;
            }
        }
        return targetAgent;
    }
}
