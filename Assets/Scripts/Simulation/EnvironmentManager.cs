using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentManager : Singleton<EnvironmentManager>
{

    public Tilemap[] maps;
    
    Dictionary<EnvironmentType, Tilemap> tileMaps;
    Dictionary<EnvironmentType, List<BarricadeObject>> objectsToBarricade;
    Dictionary<EnvironmentType, bool> availableEnvironments;

    // Start is called before the first frame update
    void Awake()
    {
        // Barricade objects
        objectsToBarricade = new Dictionary<EnvironmentType, List<BarricadeObject>>();
        BarricadeObject [] objects = FindObjectsOfType<BarricadeObject>();
        foreach(var objectToBarricade in objects)
        {
            if (objectsToBarricade.ContainsKey(objectToBarricade.environment))
                objectsToBarricade[objectToBarricade.environment].Add(objectToBarricade);
            else
            {
                objectsToBarricade.Add(objectToBarricade.environment, new List<BarricadeObject> { objectToBarricade });
            }
        }
        // Available environments
        availableEnvironments = new Dictionary<EnvironmentType, bool>();
        // Tilemaps // TODO currently not in used
        tileMaps = new Dictionary<EnvironmentType, Tilemap>();
        foreach (var tileMap in maps)
        {
            EnvironmentType type = tileMap.gameObject.GetComponent<Evironment>().environment;
            availableEnvironments.Add(type, true);
            tileMaps.Add(type, tileMap);
        }
    }

    public List<BarricadeObject> GetBarricadeObjects(EnvironmentType environment)
    {
        if (objectsToBarricade.ContainsKey(environment))
            return objectsToBarricade[environment];

        return null;
    }


    public void SetTile(TileBase tile, EnvironmentType type, Vector3 worldposition)
    {
        Vector3Int gripPosition = tileMaps[type].WorldToCell(worldposition);
        tileMaps[type].SetTile(gripPosition, tile);
    }

    public void SetTileCellPosition(TileBase tile, EnvironmentType type, Vector3Int cellPosition)
    {
        tileMaps[type].SetTile(cellPosition, tile);
    }

    public TileBase GetTileFromWorldPosition(Vector3 worldposition, EnvironmentType type)
    {
        Vector3Int gripPosition = tileMaps[type].WorldToCell(worldposition);
        return tileMaps[type].GetTile(gripPosition);
    }

    public Vector3Int GetTileCellPosition(Vector3 worldposition, EnvironmentType type)
    {
       return tileMaps[type].WorldToCell(worldposition);   
    }

    public bool IsEnvironmentAvailable(EnvironmentType environment)
    {
        if (availableEnvironments.ContainsKey(environment))
            return availableEnvironments[environment];

        Debug.LogAssertion("Environmet not exits! " + environment);
        return false;
    }

    public void LockEnvironment(EnvironmentType environment, bool available)
    {
        if (availableEnvironments.ContainsKey(environment))
            availableEnvironments[environment] = available;
    }



}
