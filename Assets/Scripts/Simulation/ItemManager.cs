using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ItemManager : Singleton<ItemManager>
{
    public Collider2D wallCollider;
    HashSet<Vector2> positionsTracker ;
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
        var objectsClose = positionsTracker.Where(x => Vector2.Distance(position, x) < distance && wallCollider.bounds.Contains(x));
        return objectsClose.Count() > 0;
    }
}
