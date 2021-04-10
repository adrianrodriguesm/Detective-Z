using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public float maxScale = 1.5f;
    public float tweenTime = 0.75f;
    Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        Debug.Log("Enter " + gameObject.name);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * maxScale, tweenTime).setEaseInSine();
    }
    /**/
    private void OnMouseExit()
    {
        Debug.Log("Exit " + gameObject.name);
        LeanTween.scale(gameObject, localScale, tweenTime).setEaseInSine();
    }
}
