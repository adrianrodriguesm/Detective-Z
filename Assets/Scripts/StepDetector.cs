using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDetector : MonoBehaviour
{
    public StepType Step;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.Step = Step;
        }
    }
    /** /
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.Step = Step;
        }
    }
    /**/
}
