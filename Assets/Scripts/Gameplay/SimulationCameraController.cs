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
    void Start()
    {
        agents = new List<AIAgent>();
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        pixelPerfectCamera.assetsPPU = 8;
        infected = StoryManager.Instance.Infected;

    }

    // Update is called once per frame
    void Update()
    {
        if (!StoryManager.Instance.IsSimulationEnd())
        {
            agents = StoryManager.Instance.AIAgents;
            Vector3 position = transform.position;
            foreach (var agent in agents)
            {
                position += agent.transform.position;
            }

            if(infected.gameObject.activeSelf)
            {
                position = infected.transform.position;
                //position = position / (agents.Count() + 2);
            }
            else
                position = position / (agents.Count() + 1);


            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            float x = Mathf.Lerp(transform.position.x, position.x, positionLerpPct);
            float y = Mathf.Lerp(transform.position.y, position.y, positionLerpPct);

            transform.position = new Vector3(x, y, -1f);


        }
    }
}
