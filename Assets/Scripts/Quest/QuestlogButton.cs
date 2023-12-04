using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestlogButton : MonoBehaviour
{
    [HideInInspector] public QuestBase myQuest;
    public TextMeshProUGUI questNameText;
    public Image uncheckBox;
    public Sprite checkedBox;

    public void SetQuest(QuestBase newQuest)
    {
        myQuest = newQuest;
        questNameText.text = newQuest.questName;
    }

    public void OnPressed()
    {
        if(GameManager.manager != null && GameManager.manager.triggeredQuests != null &&
            GameManager.manager.triggeredQuests.Contains(myQuest))
        {
            QuestBase triggeredQuest = Array.Find(GameManager.manager.triggeredQuests, quest => quest == myQuest);
            QuestlogManager.instance.UpdateQuestlogUI(myQuest, myQuest.GetObjectiveList());
        }
        else if(GameManager.manager != null && GameManager.manager.rewardReadyQuests != null &&
            GameManager.manager.rewardReadyQuests.Contains(myQuest))
        {
            QuestBase rewardReadyQuest = Array.Find(GameManager.manager.rewardReadyQuests, quest => quest == myQuest);
            QuestlogManager.instance.UpdateQuestlogUI(myQuest, myQuest.GetCompletedObjectiveList());
        }
        else if(GameManager.manager != null && GameManager.manager.completedQuests != null &&
            GameManager.manager.completedQuests.Contains(myQuest))
        {
            QuestBase completedQuest = Array.Find(GameManager.manager.completedQuests, quest => quest == myQuest);
            QuestlogManager.instance.UpdateQuestlogUI(myQuest, myQuest.GetCompletedObjectiveList());
        }
    }

    private void OnEnable()
    {
        if(myQuest.isCompleted)
        {
            uncheckBox.sprite = checkedBox;
        }
    }
}
