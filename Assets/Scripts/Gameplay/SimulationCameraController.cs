using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SimulationCameraController : MonoBehaviour
{

    List<AIAgent> agents;
    PixelPerfectCamera pixelPerfectCamera;
    InfectedAgent infected;
    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
    public float positionLerpTime = 0.2f;
    StoryManager m_StoryManager;
    void Start()
    {
        agents = new List<AIAgent>();
        m_StoryManager = StoryManager.Instance;
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        pixelPerfectCamera.assetsPPU = 4;
        infected = m_StoryManager.Infected;

    }
    /** /
    // Update is called once per frame
    void Update()
    {

        agents = m_StoryManager.AIAgents;
        Vector3 position = transform.position;
        foreach (var agent in agents)
            position += agent.transform.position;
            

        if(infected != null && infected.gameObject.activeSelf)
            position = infected.transform.position;
        else
            position = position / (agents.Count() + 1);


        var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
        float x = Mathf.Lerp(transform.position.x, position.x, positionLerpPct);
        float y = Mathf.Lerp(transform.position.y, position.y, positionLerpPct);

        transform.position = new Vector3(x, y, -1f);


        
    }
    /**/
}
