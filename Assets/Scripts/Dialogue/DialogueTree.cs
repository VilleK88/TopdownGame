using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogues/Dialogue Basic")]
public class DialogueTree : ScriptableObject
{
    public DialogueSection[] sections;
}
