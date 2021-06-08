using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ItemManager : Singleton<ItemManager>
{
    public List<Collider2D> wallCollider;
    HashSet<Vector2> positionsTracker;
    private void Start()
    {
        positionsTracker = new HashSet<Vector2>();
    }
    public HashSet<Vector2> Positions
    {
        get { return positionsTracker; }
    }
    public float distance = 0.5f;

    public bool IsValidPosition(Vector2 position)
    {
        var objectsClose = positionsTracker.Where(x => Vector2.Distance(position, x) < distance);
        var collideWithPoint = wallCollider.Where(collider => collider.OverlapPoint(position));
        return objectsClose.Count() > 0 || collideWithPoint.Count() > 0;
    }

    public bool IsValidPosition(Vector2 position, Collider collider)
    {
        var objectsClose = positionsTracker.Where(x => Vector2.Distance(position, x) < distance);
        Vector2 minBounds = collider.bounds.min;
        Vector2 maxBounds = collider.bounds.max;
        bool isOverlaping = false;
        for(int x = (int)minBounds.x; x < maxBounds.x; x++)
        {
            for (int y = (int)minBounds.y; x < maxBounds.y; y++)
                isOverlaping = wallCollider.Where(collider => collider.OverlapPoint(position)).Count() > 0;
            if (isOverlaping)
                break;
        }
        //var collideWithPoint = wallCollider.Where(collider => collider.OverlapPoint(position));
        return objectsClose.Count() > 0 || isOverlaping;
    }

    public bool IsValidPosition(Vector2 position, float distance)
    {
        var objectsClose = positionsTracker.Where(x => Vector2.Distance(position, x) < distance);
        var collideWithPoint = wallCollider.Where(collider => collider.OverlapPoint(position));
        return objectsClose.Count() > 0 || collideWithPoint.Count() > 0;
    }

    public bool CheckIfOverlapWithWall(Vector2 position)
    {
        var collideWithPoint = wallCollider.Where(collider => collider.OverlapPoint(position));
        return collideWithPoint.Count() > 0;
    }
}
