using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BarricadeObject : MonoBehaviour
{
    public GameObject tilemapBeforeMoving;
    public GameObject tilemapAfterMoving;
    public Transform transformObj;
    public EnvironmentType environment;
    
    // Start is called before the first frame update
    void Awake()
    {
        tilemapAfterMoving.SetActive(true);
        tilemapAfterMoving.SetActive(false);
    }

    public void MoveObject()
    {
        tilemapBeforeMoving.SetActive(false);
        tilemapAfterMoving.SetActive(true);
    }
}
