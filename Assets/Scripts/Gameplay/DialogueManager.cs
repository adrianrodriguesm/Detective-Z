﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public Text titleText;
    public Text DialogueText;
    Queue<string> sentences;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void BeginDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting dialogue " + dialogue.title);
        animator.SetBool("IsOpen", true);
        titleText.text = dialogue.title;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            animator.SetBool("IsOpen", false);
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        yield return new WaitForSeconds(0.5f);
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(4f);
        DisplayNextSentence();
    }
}