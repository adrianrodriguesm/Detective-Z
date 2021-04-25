using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public GameObject player;
    CameraController2D cameraController;
    bool isGameplayStated;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        player.SetActive(false);
        cameraController = Camera.main.GetComponent<CameraController2D>();
        isGameplayStated = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("EndGame"))
            Application.Quit();

        if (StoryManager.Instance.IsSimulationEnd())
        {
            Cursor.visible = true;
            if (!isGameplayStated)
            {
                isGameplayStated = true;
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
}
