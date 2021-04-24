using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public float maxScale = 1.5f;
    public float tweenTime = 0.75f;
    Vector3 localScale;
    bool isEnable = true;
    public bool Enable
    {
        get { return isEnable; }
        set { isEnable = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
    }

    void OnMouseEnter()
    {
        if (!isEnable)
            return;
        //Debug.Log("Enter " + gameObject.name);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * maxScale, tweenTime).setEaseInSine();
    }
    /**/
    private void OnMouseExit()
    {
        if (!isEnable)
            return;
        //Debug.Log("Exit " + gameObject.name);
        LeanTween.scale(gameObject, localScale, tweenTime).setEaseInSine();
    }
}
