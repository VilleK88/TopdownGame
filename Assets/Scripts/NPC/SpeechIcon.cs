using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechIcon : MonoBehaviour
{
    public Transform targetNPC;
    Vector3 offset = new Vector3(-1.5f, 2, 1.5f);

    private void Update()
    {
        
        if(targetNPC != null)
        {
            transform.position = targetNPC.position + offset;
        }
    }
}
