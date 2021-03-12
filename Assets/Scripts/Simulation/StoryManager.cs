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
        get { return infected; }
        set { infected = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        agents = FindObjectsOfType<AIAgent>().ToList();
        deadAgents = new List<AIAgent>();
        infected = FindObjectOfType<InfectedAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var agent in agents)
        {
           if(agent.IsDead())
            {
                agents.Remove(agent);
                deadAgents.Add(agent);
            }
        }
    }
}
