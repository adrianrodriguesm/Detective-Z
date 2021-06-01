using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Vector2 m_PreviousMovement;
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
        // Input
        m_PreviousMovement = movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
    }

    void FixedUpdate()
    {
        // Movement
        if(m_StoryManager.AnimationState.Equals(AnimationState.Stop))
        {
            animator.SetFloat("Speed", movement.sqrMagnitude);
            if (movement.sqrMagnitude <= 0.001f)
            {
                animator.SetFloat("Horizontal", m_PreviousMovement.x);
                animator.SetFloat("Vertical", m_PreviousMovement.y);
                movement = m_PreviousMovement;
                isIdle = true;
            }
            else
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                isIdle = false;
            }

            if(!isIdle)
                rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        }
           
       
        
    }

    public void ChangedEnvironment(EnvironmentType type)
    {
        AudioPlayer player = m_SoundManager.AudioPlayer;
        environment = type;
        if (type.Equals(EnvironmentType.Garden))
        {
            m_SoundManager.PlayOutsideAmbientSound();
        }
        else
        {
            if(!player.IsLoopPlaying("AmbientWindInside"))
            {
                m_SoundManager.PlayInsideAmbientSound();
            }
               
        }
    }

    public void PlayStepSound()
    {
        Debug.Log("Step in " + environment);
        if (environment.Equals(EnvironmentType.Garden))
            m_AudioPlayer.PlayOnceRandomClip("Grass");
        else
            m_AudioPlayer.PlayOnceRandomClip("Wood");

    }
}
