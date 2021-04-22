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
    UnityTemplateProjects.SimpleCameraController simpleCamera;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        player.SetActive(false);
        cameraController = Camera.main.GetComponent<CameraController2D>();
       // simpleCamera = Camera.main.GetComponent<UnityTemplateProjects.SimpleCameraController>();
       // cameraController.enabled = t;
    }

    private void Update()
    {
        if(StoryManager.Instance.IsSimulationEnd())
        {
            //simpleCamera.enabled = false;
            //cameraController.enabled = true;
            player.SetActive(true);
        }
    }



}
