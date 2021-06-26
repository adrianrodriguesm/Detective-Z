using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashObject : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    InfectedAgent m_Infected;
    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.enabled = false;
    }

    private void Start()
    {
        m_Infected = StoryManager.Instance.Infected;
    }
    private void Update()
    {
        if(m_Infected != null && !m_SpriteRenderer.enabled && Vector2.Distance(m_Infected.transform.position, transform.position) < 2f)
            m_SpriteRenderer.enabled = true;
    }
}
