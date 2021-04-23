using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedRadius : MonoBehaviour
{
    InfectedAgent infected;
    // Start is called before the first frame update
    void Start()
    {
        infected = StoryManager.Instance.Infected;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Agent") && !(infected.Action is AttackAgent))
        {
            Debug.Log("DETECTED AGENT!");
            infected.Action = new AttackAgent(infected, collision.GetComponent<AIAgent>());
        }
    }
}
