using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBishopHenry : MonoBehaviour
{
    [SerializeField] QuestBase killHenryQuest;

    private void Start()
    {
        killHenryQuest.initializeQuest();
    }
}
