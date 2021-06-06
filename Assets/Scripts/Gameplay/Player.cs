using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StepType
{
    None, Blood, Glass, Fence
}

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Animator animator;
    Vector2 movement;
    public EnvironmentType environment;
    bool isIdle = false;
    AudioPlayer m_AudioPlayer;
    StoryManager m_StoryManager;
    SoundManager m_SoundManager;
    StepType currOvelapStep;
    public StepType Step
    {
        get { return currOvelapStep; }
        set { currOvelapStep = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        movement = Vector2.zero;
        m_AudioPlayer = GetComponent<AudioPlayer>();
        m_StoryManager = StoryManager.Instance;
        m_SoundManager = SoundManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_StoryManager.AnimationState.Equals(AnimationState.Stop))
            return;
        
        // Input
        Vector2 lastMovement = movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if(movement.sqrMagnitude <= 0.001f)
        {
            animator.SetFloat("Horizontal", lastMovement.x);
            animator.SetFloat("Vertical", lastMovement.y);
            movement = lastMovement;
            isIdle = true;
        }
        else
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            isIdle = false;
        }
    }

    void FixedUpdate()
    {
        // Movement
        if (m_StoryManager.AnimationState.Equals(AnimationState.Stop) && !isIdle)
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position);
    }

    public void ChangedEnvironment(EnvironmentType type)
    {
        AudioPlayer player = m_SoundManager.AudioPlayer;
        environment = type;
        if (type.Equals(EnvironmentType.Garden))
            m_SoundManager.PlayOutsideAmbientSound();
        else if(!player.IsLoopPlaying("AmbientWindInside"))
            m_SoundManager.PlayInsideAmbientSound(); 
    }

    public void PlayStepSound()
    {
        //Debug.Log("STEPP TYPE : " + currOvelapStep);
        switch (currOvelapStep)
        {
            
            case StepType.Blood: m_AudioPlayer.PlayOnceRandomClip("BloodStep"); break;
            case StepType.Fence: m_AudioPlayer.PlayOnceRandomClip("FenceStep"); break;
            case StepType.Glass: m_AudioPlayer.PlayOnceRandomClip("GlassStep"); break;
                //case StepType.Glass: break;
        }
        if (environment.Equals(EnvironmentType.Garden))
        {
            m_AudioPlayer.PlayOnceRandomClip("GrassRegular"); 
        }  
        else if(environment.Equals(EnvironmentType.Bathroom))
        {
            m_AudioPlayer.PlayOnceRandomClip("BathroomRegular");
            m_AudioPlayer.PlayOnceRandomClip("WoodRegular");
        }
        else if(environment.Equals(EnvironmentType.Kitchen))
        {
            //m_AudioPlayer.PlayOnceRandomClip("BathroomRegular");
            m_AudioPlayer.PlayOnceRandomClip("WoodRegular");
        }
        else
        {
            m_AudioPlayer.PlayOnceRandomClip("WoodRegular");
        }
        currOvelapStep = StepType.None;
    }
}
