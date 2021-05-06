using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public Dialogue dialogue;
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
        if (!isEnable || !StoryManager.Instance.IsSimulationEnd())
            return;
        //Debug.Log("Enter " + gameObject.name);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * maxScale, tweenTime).setEaseInSine();
        DialogueManager.Instance.BeginDialogue(dialogue);
    }
    /**/
    private void OnMouseExit()
    {
        if (!isEnable || !StoryManager.Instance.IsSimulationEnd())
            return;
        
        LeanTween.scale(gameObject, localScale, tweenTime).setEaseInSine();
        DialogueManager.Instance.EndDialogue();
    }
}
