using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSceneChange : MonoBehaviour
{
    void Start()
    {
        //QuestManager.questManager.RemoveCompletedQuestIDs();
        //QuestManager.questManager.RemoveAllCompletedQuestsOnStart();
        QuestManager.questManager.RemoveEnemiesForRewardReadyQuests();
        QuestManager.questManager.RemoveEnemiesForCompletedQuests();
    }
}
