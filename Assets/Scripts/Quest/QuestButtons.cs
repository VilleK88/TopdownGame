using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButtons : MonoBehaviour
{
    public void AcceptQuest()
    {
        QuestManager.questManager.currentQuest.initializeQuest();
        QuestManager.questManager.StartQuestBuffer();
        QuestManager.questManager.questUI.SetActive(false);
    }

    public void DeclineQuest()
    {
        QuestManager.questManager.StartQuestBuffer();
        QuestManager.questManager.questUI.SetActive(false);
    }
}