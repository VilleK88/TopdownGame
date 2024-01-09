using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneDialogueManager : MonoBehaviour
{
    #region Singleton
    public static SceneDialogueManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    Queue<string> sentences;

    public bool inSceneDialogue = false;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartSceneDialogue(SceneDialogue sceneDialogue)
    {
        inSceneDialogue = true;

        nameText.text = sceneDialogue.name;

        sentences.Clear();

        foreach(string sentence in sceneDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndSceneDialogue();
            inSceneDialogue = false;
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndSceneDialogue()
    {
        Debug.Log("End of scene conversation.");
    }
}
