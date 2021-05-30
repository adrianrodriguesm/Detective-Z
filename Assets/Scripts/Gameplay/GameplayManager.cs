using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public GameObject player;
    bool isGameplayStarted;
    int counter = 0;
    public bool GameplayStarted
    {
        get { return isGameplayStarted; }
    }
    float gameplayDuration = 0;
    public float GameplayDuration
    {
        get { return gameplayDuration; }
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        player.SetActive(false);

        isGameplayStarted = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("EndGame"))
        {
            GameplaySerialize serialize = GenerateSerializeData();
            Serializer.Serialize(serialize);
            Application.Quit();
        }
            

        if (StoryManager.Instance.IsSimulationEnd())
        {
            if(counter <= 0)
            {
                counter++;
                return;
            }
            Cursor.visible = true;
            gameplayDuration += Time.deltaTime;
            if (!isGameplayStarted)
            {
                isGameplayStarted = true;
                //SetAudioMute(true);
                player.SetActive(true);
            }
        }
        else
        {
            //Cursor.lockState = CursorLockMode.locked;
            Cursor.visible = false;
            //SetAudioMute(false);
        }


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
