using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

enum AnimationState
{
    Play, Stop, Pause, Rewind
}

public class StoryManager : Singleton<StoryManager>
{
    List<AIAgent> agents;
    List<AIAgent> deadAgents;
    [HideInInspector]
    public List<AIAgent> AIAgents
    {
        get { return agents; }
    }
    public List<AIAgent> DeadAIAgents
    {
        get { return deadAgents; }
    }
    InfectedAgent infected;
    public InfectedAgent Infected
    {
        get { return infected;  }
        set { infected = value; }
    }

    public float timerToStartTheAttack = 0f;
    [Range(1,5)]
    public float timeScale;
    float fixedDeltaTime;
   // float frameCounter = 0;

    List<Sprite> spriteFrame;
    int currIndex = 0;
    public Image imageDisplay;
    AnimationState animationState;
    float timer = 0;
    public float timerForReplay;
    List<InfectedEntrance> infectedEntrance;
    public List<InfectedEntrance> ExitPoints
    {
        get { return infectedEntrance; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        agents = FindObjectsOfType<AIAgent>().ToList();
        deadAgents = new List<AIAgent>();
        infected = FindObjectOfType<InfectedAgent>();
        infectedEntrance = FindObjectsOfType<InfectedEntrance>().ToList();
        StartCoroutine(PrepareInfectedAttack());
        Time.timeScale = timeScale;
        fixedDeltaTime = Time.fixedDeltaTime;
        Time.fixedDeltaTime *= Time.timeScale;
        spriteFrame = new List<Sprite>();
        SimulationLoader.Instance.gameObject.SetActive(true);
    }

    private void Start()
    {
        imageDisplay.enabled = false;
        animationState = AnimationState.Stop;
        timer = 0;
       
    }
    IEnumerator PrepareInfectedAttack()
    {
        infected.gameObject.SetActive(false);
        yield return new WaitForSeconds(timerToStartTheAttack);
        
        InfectedEntrance entrancePoint = infectedEntrance[Random.Range(0, infectedEntrance.Count())];
        infected.transform.position = entrancePoint.transform.position;
        infected.gameObject.SetActive(true);
        entrancePoint.Entry();  
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSimulationEnd())
        {
            
            Time.fixedDeltaTime = fixedDeltaTime;
            Time.timeScale = 1f;
            if (Input.GetButtonDown("PlaySimulatedStory"))
                animationState = AnimationState.Play;
            else if (Input.GetButtonDown("StopSimulatedStory"))
                animationState = AnimationState.Stop;
            else if (Input.GetButtonDown("RewindSimulatedStory"))
                animationState = AnimationState.Rewind;
            else if (Input.GetButtonDown("PauseSimulatedStory"))
                animationState = AnimationState.Pause;

            switch (animationState)
            {
                case AnimationState.Play:
                    PlaySimulation(); break;
                case AnimationState.Stop:
                    StopSimulation(); break;
                case AnimationState.Rewind:
                    RewindSimulation(); break;
                case AnimationState.Pause:
                    PauseSimulation(); break;
            }
            return;
        }

        foreach (var agent in agents.ToList())
        {
           if(agent.IsDead())
           {
               agents.Remove(agent);
               deadAgents.Add(agent);
           }
        }
        if (agents.Count == 0 && infected.IsDeadOrEscaped())
        {
            infected.DisableCollider();
            // Generated sprite with scars due the different types of attacks
            infected.enabled = false;
        }
        // Record story throught the generation of several frames
        StartCoroutine(CaptureFrame());

    }
    // Take a shot immediately
    IEnumerator CaptureFrame()
    {
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();
        //simulationCamera.enabled = true;
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        RenderTexture rt = new RenderTexture(width, height, 24);
        Camera.main.targetTexture = rt;
        // Set background color to black
        Camera.main.backgroundColor = Color.black;
        Camera.main.allowMSAA = false;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        Camera.main.Render();
        RenderTexture.active = rt;
        // Read screen contents into the texture
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();
        Camera.main.targetTexture = null;
        // Encode texture into PNG
        //byte[] bytes = texture.EncodeToPNG();
        Sprite frame = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                                new Vector2(0.5f, 0.5f), 8);
        spriteFrame.Add(frame);
        // Destroy(texture);
        //For testing purposes, also write to a file in the project folder
        //File.WriteAllBytes(Application.dataPath + "/SimulationFrames/SavedScreen" + ".png", bytes);
        //simulationCamera.enabled = false;
        yield return null;
    }
    public bool SomeAgentDied()
    {
        return deadAgents.Count() > 0;
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

    void PlaySimulation()
    {
        timer += Time.unscaledDeltaTime;
        if(timer > timerForReplay)
        {
            imageDisplay.enabled = true;
            if (currIndex >= spriteFrame.Count())
                currIndex = spriteFrame.Count() -1;
            else if (currIndex <= 0)
                currIndex = 0;

            imageDisplay.sprite = spriteFrame[currIndex++];
            timer = 0;
        }
        
    }
    void RewindSimulation()
    {
        timer += Time.unscaledDeltaTime;
        if (timer > timerForReplay)
        {
            imageDisplay.enabled = true;
            if (currIndex <= 0)
                currIndex = 0;
            else if (currIndex >= spriteFrame.Count())
                currIndex = spriteFrame.Count() - 1;

            imageDisplay.sprite = spriteFrame[currIndex--];
            timer = 0;
        }
    }

    void StopSimulation()
    {
        currIndex = 0;
        imageDisplay.enabled = false;
    } 
    void PauseSimulation()
    {
        
    }
}
