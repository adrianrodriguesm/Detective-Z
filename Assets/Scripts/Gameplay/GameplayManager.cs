using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public List<GameObject> ActiveWhenGameplayStart;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public GameObject player;
    bool isGameplayStarted;
    StoryManager m_StoryManager;
    SimulationCameraController m_SimulationCameraController;
    CameraController2D m_GameplayCameraController;
    bool isPlayerActive = false;
    public bool GameplayStarted
    {
        get { return isGameplayStarted; }
    }
    float gameplayDuration = 0;
    public float GameplayDuration
    {
        get { return gameplayDuration; }
    }

    private void Awake()
    {
        foreach (var gameObject in ActiveWhenGameplayStart)
            gameObject.SetActive(false);

        m_SimulationCameraController = FindObjectOfType<SimulationCameraController>();
        m_GameplayCameraController = FindObjectOfType<CameraController2D>();
        m_GameplayCameraController.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        player.SetActive(false);
        Cursor.visible = false;
        isGameplayStarted = false;
        m_StoryManager = StoryManager.Instance;
       

    }

    private void Update()
    {
        if (Input.GetButtonDown("EndGame"))
        {
            GameplaySerialize serialize = GenerateSerializeData();
            Serializer.Serialize(serialize);
            Application.Quit();
        }
            

        if (m_StoryManager.IsSimulationEnd())
        {
            gameplayDuration += Time.deltaTime;
            if (!isGameplayStarted)
            {
               
                StartCoroutine(ActivateGameplayObjects());
            }
            else if(isGameplayStarted && !isPlayerActive)
            {
                SoundManager.Instance.DestroyAudioListener();
                player.SetActive(true);
            }
               
        }
    }

    IEnumerator ActivateGameplayObjects()
    {
        for(int i = 0; i < 2; i++)
            yield return new WaitForEndOfFrame();

        isGameplayStarted = true;  
        foreach (var gameObject in ActiveWhenGameplayStart)
            gameObject.SetActive(true);

        

        m_SimulationCameraController.enabled = false;
        m_GameplayCameraController.enabled = true;
        Cursor.visible = true;
    }
    GameplaySerialize GenerateSerializeData()
    {
        GameplaySerialize serializeObject = new GameplaySerialize();
        serializeObject.GameplayDuration = gameplayDuration;
        serializeObject.Clues = new List<ClueSerialize>();
        List<Clue> clues = FindObjectsOfType<Clue>().ToList();
        foreach(var clue in clues)
        {
            ClueSerialize serializeClue = clue.GenerateClueSerialize();
            if (serializeClue.NumberOfInteraction > 0)
                serializeObject.NumberOfClueWithInteraction++;
            else
                serializeObject.NumberOfClueWithoutInteraction++;

            serializeObject.Clues.Add(serializeClue);
        }
        return serializeObject;
    }
}
