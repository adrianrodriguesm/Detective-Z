using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationLoader : MonoBehaviour
{

    Slider sliderLoader;
    float progress = 0;
    // Start is called before the first frame update
    void Start()
    {
        sliderLoader = GameObject.Find("StoryLoaderBar")?.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(StoryManager.Instance.IsSimulationEnd())
            sliderLoader.gameObject.SetActive(false);

        if (Input.GetButtonDown("SeeSimulation"))
            sliderLoader.gameObject.SetActive(false);
        else if (Input.GetButtonDown("SeeLoader"))
            sliderLoader.gameObject.SetActive(true);

        sliderLoader.value = ObtainLoaderProgress();

    }

    float ObtainLoaderProgress()
    {
        progress+=0.1f;
        return Mathf.Clamp01(progress);
    }
}
