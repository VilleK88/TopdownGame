using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestlogButton : MonoBehaviour
{
    [HideInInspector] public QuestBase myQuest;
    public TextMeshProUGUI questNameText;

    public void SetQuest(QuestBase newQuest)
    {
        myQuest = newQuest;
        questNameText.text = newQuest.questName;
    }

    public void OnPressed()
    {
        QuestlogManager.instance.UpdateQuestlog(myQuest);
    }
}
