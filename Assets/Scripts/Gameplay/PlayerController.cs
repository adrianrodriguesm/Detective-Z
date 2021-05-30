using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Animator animator;
    Vector2 movement;
    public EnvironmentType environment;
    bool isIdle = false;
    // Start is called before the first frame update
    void Start()
    {
        movement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
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
        if(!isIdle)
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
       // Debug.Log(environment);
        
    }

    public void ChangedEnvironment(EnvironmentType type)
    {
        Debug.Log("ChangedEnviroment " + type);
        AudioPlayer player = SoundManager.Instance.AudioPlayer;
        environment = type;
        if (type.Equals(EnvironmentType.Garden))
        {
            SoundManager.Instance.PlayOutsideAmbientSound();
        }
        else
        {
            if(!player.IsLoopPlaying("AmbientWindInside"))
            {
                SoundManager.Instance.PlayInsideAmbientSound();
            }
               
        }
    }
}
