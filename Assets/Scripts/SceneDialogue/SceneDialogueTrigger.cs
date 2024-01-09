using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour
{
    public SceneDialogue sceneDialogue;
    bool ifInSceneDialogue;
    
    public void TriggerSceneDialogue()
    {
        FindObjectOfType<SceneDialogueManager>().StartSceneDialogue(sceneDialogue);
    }

    private void Start()
    {
        //TriggerSceneDialogue();
    }

    private void Update()
    {
        ifInSceneDialogue = SceneDialogueManager.instance.inSceneDialogue;

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!ifInSceneDialogue)
            {
                TriggerSceneDialogue();
            }
            else
            {
                SceneDialogueManager.instance.DisplayNextSentence();
            }
        }
    }
}
