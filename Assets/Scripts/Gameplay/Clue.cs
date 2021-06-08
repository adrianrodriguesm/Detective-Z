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
    SpriteRenderer m_SpriteRenderer;
    int m_DefaultLayer;
    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_DefaultLayer = m_SpriteRenderer.sortingOrder;

    }
    StoryManager m_StoryManager;
    DialogueManager m_DialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        m_StoryManager = StoryManager.Instance;
        m_DialogueManager = DialogueManager.Instance;
    }

    private void Update()
    {
        if (isInteracting)
            interactionDuration += Time.deltaTime;

    }

    void OnMouseEnter()
    {
        if (!isEnable || !m_StoryManager.IsSimulationEnd() || !m_StoryManager.AnimationState.Equals(AnimationState.Stop))
            return;
        //Debug.Log("Enter " + gameObject.name);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * maxScale, tweenTime).setEaseInSine();
        m_DialogueManager.BeginDialogue(dialogue);
        numberOfInteractions++;
        isInteracting = true;
        m_SpriteRenderer.sortingOrder = 20;
        //Debug.Log("Layer Entry: " + m_SpriteRenderer.sortingOrder);
    }

    private void OnMouseOver()
    {
        if ( !m_StoryManager.AnimationState.Equals(AnimationState.Stop))
        {
            LeanTween.cancel(gameObject);
            m_DialogueManager.EndDialogue();
            isInteracting = false;
        }
           
    }
    /**/
    private void OnMouseExit()
    {
        m_SpriteRenderer.sortingOrder = m_DefaultLayer;
        if (!isEnable || !m_StoryManager.IsSimulationEnd() || !m_StoryManager.AnimationState.Equals(AnimationState.Stop))
            return;
        
        LeanTween.scale(gameObject, localScale, tweenTime).setEaseInSine();
        m_DialogueManager.EndDialogue();
        isInteracting = false;
        //Debug.Log("Layer Exit: " + m_SpriteRenderer.sortingOrder);
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
