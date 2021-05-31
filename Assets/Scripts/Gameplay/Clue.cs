using System;
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
    int numberOfInteractions = 0;
    public int NumberOfInteractions
    {
        get { return numberOfInteractions; }
    }
    float interactionDuration = 0f;
    public float InteractionsTotalDuration
    {
        get { return interactionDuration; }
    }
    public bool Enable
    {
        get { return isEnable; }
        set { isEnable = value; }
    }
    bool isInteracting = false;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
    }

    private void Update()
    {
        if (isInteracting)
            interactionDuration += Time.deltaTime;

    }

    void OnMouseEnter()
    {
        if (!isEnable || !StoryManager.Instance.IsSimulationEnd() || !StoryManager.Instance.AnimationState.Equals(AnimationState.Stop))
            return;
        //Debug.Log("Enter " + gameObject.name);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * maxScale, tweenTime).setEaseInSine();
        DialogueManager.Instance.BeginDialogue(dialogue);
        numberOfInteractions++;
        isInteracting = true;
    }

    private void OnMouseOver()
    {
        if ( !StoryManager.Instance.AnimationState.Equals(AnimationState.Stop))
        {
            LeanTween.cancel(gameObject);
            DialogueManager.Instance.EndDialogue();
            isInteracting = false;
        }
           
    }
    /**/
    private void OnMouseExit()
    {
        if (!isEnable || !StoryManager.Instance.IsSimulationEnd() || !StoryManager.Instance.AnimationState.Equals(AnimationState.Stop))
            return;
        
        LeanTween.scale(gameObject, localScale, tweenTime).setEaseInSine();
        DialogueManager.Instance.EndDialogue();
        isInteracting = false;
    }

    public ClueSerialize GenerateClueSerialize()
    {
        ClueSerialize serialize;
        serialize.ClueName = gameObject.name;
        serialize.NumberOfInteraction = NumberOfInteractions;
        serialize.Duration = interactionDuration;
        return serialize;
    }
}
