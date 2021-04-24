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
       // simpleCamera = Camera.main.GetComponent<UnityTemplateProjects.SimpleCameraController>();
       // cameraController.enabled = t;
    }

    private void Update()
    {
        if(StoryManager.Instance.IsSimulationEnd())
        {
            if (!isGameplayStated)
            {
                isGameplayStated = true;
                player.SetActive(true);
            }

            if(Input.GetButtonDown("EndGame"))
                Application.Quit();
        }
      
    }



}
