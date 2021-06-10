using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SimulationLoader : Singleton<SimulationLoader>
{
    InfectedAgent infected;
    Slider sliderLoader;
    public Text text;
    float progress = 0;
    float step;
    int numberOfDeadAgents;
    bool attackStarted;
    GameObject loaderCanvas;
    // Start is called before the first frame update
    void Start()
    {
        numberOfDeadAgents = 0;
        sliderLoader = GameObject.Find("StoryLoaderBar")?.GetComponent<Slider>();
        loaderCanvas = gameObject;
        int numberOfSteps = StoryManager.Instance.AIAgents.Count();
        infected = StoryManager.Instance.Infected;
        // Start Attack and Escape of infected
        numberOfSteps += 2;
        step = (float)1 / (float)numberOfSteps;
        attackStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameplayManager.Instance.GameplayStarted)
            loaderCanvas.SetActive(false);
        else
            UpdateProgress();

        if (Input.GetButtonDown("SeeSimulation"))
        {
            for(int i = 0; i < loaderCanvas.transform.childCount; i++)
            {
                var child = loaderCanvas.transform.GetChild(i).gameObject;
                if (child)
                    child.SetActive(false);
            }
        }
           
        else if (Input.GetButtonDown("SeeLoader"))
        {
            for (int i = 0; i < loaderCanvas.transform.childCount; i++)
            {
                var child = loaderCanvas.transform.GetChild(i).gameObject;
                if (child)
                    child.SetActive(true);
            }
        }
           
    }

    void UpdateProgress()
    {
        if(!attackStarted && infected.gameObject.activeSelf)
        {
            attackStarted = true;
            progress += step;
        }
        if(infected.IsDeadOrEscaped())
            progress += step;

        int currNumberOfDeadAgents = StoryManager.Instance.DeadAIAgents.Count();
        if(numberOfDeadAgents != currNumberOfDeadAgents)
        {
            int numberOfNewDeadAgents = currNumberOfDeadAgents - numberOfDeadAgents;
            progress += (step * numberOfNewDeadAgents);
            numberOfDeadAgents = currNumberOfDeadAgents;
        }
        sliderLoader.value = Mathf.Clamp01(progress);
        text.text = "generating story.." + (progress * 100) + "%";
    }
}
