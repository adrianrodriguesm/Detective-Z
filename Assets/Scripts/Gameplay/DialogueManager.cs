using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public Text titleText;
    public Text DialogueText;
    Queue<string> sentences;
    public Animator animator;
    SoundManager m_SoundManager;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        m_SoundManager = SoundManager.Instance;
        titleText.transform.parent.gameObject.SetActive(false);
    }

    public void BeginDialogue(Dialogue dialogue)
    {
        //Debug.Log("Starting dialogue " + dialogue.title);
        //animator.SetBool("IsOpen", true);
        titleText.transform.parent.gameObject.SetActive(true);
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
        // animator.SetBool("IsOpen", false);
        titleText.transform.parent.gameObject.SetActive(false);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            //animator.SetBool("IsOpen", false);

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
            if (!titleText.transform.parent.gameObject.activeInHierarchy)
                break;

            DialogueText.text += letter;
            m_SoundManager.PlayDialogueSound();
            yield return new WaitForSeconds(0.06f);
        }
        yield return new WaitForSeconds(4f);
        DisplayNextSentence();
    }
}
