using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButtons : MonoBehaviour
{
    public void AcceptQuest()
    {
        QuestManager.questManager.currentQuestDialogueTrigger.hasActiveQuest = false;
        QuestManager.questManager.currentQuest.initializeQuest();
        QuestManager.questManager.StartQuestBuffer();
        QuestManager.questManager.QuestAcceptedSound();
        QuestManager.questManager.questUI.SetActive(false);

        ObjectiveTracker.instance.UpdateTracker($"You've accepted {QuestManager.questManager.currentQuest.questName}");
    }

    public void DeclineQuest()
    {
        QuestManager.questManager.StartQuestBuffer();
        QuestManager.questManager.questUI.SetActive(false);
    }
}
