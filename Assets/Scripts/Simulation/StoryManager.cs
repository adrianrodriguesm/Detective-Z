﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum AnimationState
{
    Play, Stop, Pause, Rewind
}
[System.Serializable]
public class ActionStorage
{
    public string Name;
    public Type ActionType;

    public ActionStorage(string name, Type actionType)
    {
        Name = name;
        ActionType = actionType;
    }
}

public class StoryManager : Singleton<StoryManager>
{
    List<AIAgent> agents;
    List<AIAgent> deadAgents;
    public List<ActionStorage> actionsExecuted;

    public List<ActionStorage> ActionsExecuted
    {
        get { return actionsExecuted; }
    }
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
    List<Sprite> spriteFrame;
    int currIndex = 0;
    public Image imageDisplay;
    AnimationState animationState;
    public AnimationState AnimationState
    {
        get { return animationState; }
    }
    float timer = 0;
    public float timerForReplay;
    List<InfectedEntrance> infectedEntrance;
    bool fisrtTime = true;
    public List<InfectedEntrance> ExitPoints
    {
        get { return infectedEntrance; }
    }
    public bool UsedRandomSeed = false;
    public int randomSeed = 5;
    SoundManager m_SoundManager;
    public InfectedEntrance ForceEntryPoint;
    GameObject m_Player;
    // Start is called before the first frame update
    void Awake()
    {
        if (UsedRandomSeed)
            UnityEngine.Random.InitState(randomSeed);
        agents = FindObjectsOfType<AIAgent>().ToList();
        deadAgents = new List<AIAgent>();
        actionsExecuted = new List<ActionStorage>();
        infected = FindObjectOfType<InfectedAgent>();
        infectedEntrance = FindObjectsOfType<InfectedEntrance>().ToList();
        StartCoroutine(PrepareInfectedAttack());
        Time.timeScale = timeScale;
        fixedDeltaTime = Time.fixedDeltaTime;
        Time.fixedDeltaTime *= Time.timeScale;
        spriteFrame = new List<Sprite>();
        m_SoundManager = SoundManager.Instance;
        SimulationLoader.Instance.gameObject.SetActive(true);
        m_Player = GameObject.FindGameObjectWithTag("Player");

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
        if(ForceEntryPoint == null)
            ForceEntryPoint = infectedEntrance[UnityEngine.Random.Range(0, infectedEntrance.Count())];
        infected.transform.position = ForceEntryPoint.transform.position;
        infected.gameObject.SetActive(true);
        ForceEntryPoint.Entry();  
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSimulationEnd())
        {
            if(fisrtTime)
            {
                fisrtTime = false;
                //simualtionDataLibrary.simulations.Add(simulationFrameData);
            }
            Time.fixedDeltaTime = fixedDeltaTime;
            Time.timeScale = 1f;
            if (Input.GetButtonDown("PlaySimulatedStory"))
            {
                animationState = AnimationState.Play;
                m_SoundManager.PlayClickSound();
                m_SoundManager.PlaySimulation();
            }
               
            else if (Input.GetButtonDown("StopSimulatedStory"))
            {
                animationState = AnimationState.Stop;
                m_SoundManager.PlayClickSound();
            }
                
            else if (Input.GetButtonDown("RewindSimulatedStory"))
            {
                animationState = AnimationState.Rewind;
                m_SoundManager.PlayClickSound();
                m_SoundManager.RewindSimulation();
            }
                
            else if (!animationState.Equals(AnimationState.Stop) && Input.GetButtonDown("PauseSimulatedStory"))
            {
                animationState = AnimationState.Pause;
                m_SoundManager.PlayClickSound();
                m_SoundManager.PlaySimulation();
            }
                

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
            
        }
        else
        {
            foreach (var agent in agents.ToList())
            {
                if (agent.IsDead())
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
        }
        if(!GameplayManager.Instance.GameplayStarted && !m_Player.activeSelf)
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
                                new Vector2(0.5f, 0.5f), 16);
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
        //SoundManager.Instance.RewindSimulation();
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
        SoundManager.Instance.StopSimulation();
        currIndex = 0;
        imageDisplay.enabled = false;
    } 
    void PauseSimulation()
    {
        
    }
    public void AddExecutedAction(Action action)
    {
        ActionStorage actionStorage = new ActionStorage(action.name, action.GetType());
        ActionsExecuted.Add(actionStorage);
    }
    public bool WasActionExecuted(Action action)
    {
        bool result = ActionsExecuted.Where(x => String.Equals(x.Name, action.name)).Count() > 0;
        return result;
    }

    public bool IsAgentNear(AIAgent agent, float distance)
    {
        foreach(var agentInList in agents)
        {
            if (agent != agentInList && Vector2.Distance(agent.transform.position, agentInList.transform.position) < distance)
                return true;
        }
        foreach (var agentInList in deadAgents)
        {
            if (agent != agentInList && Vector2.Distance(agent.transform.position, agentInList.transform.position) < distance)
                return true;
        }
        return false;
    }
}
