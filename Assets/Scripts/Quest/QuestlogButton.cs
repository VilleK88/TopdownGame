using System.Collections;
using System.Collections.Generic;
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
        QuestlogManager.instance.UpdateQuestlogUI(myQuest, myQuest.GetObjectiveList());
    }

    private void OnEnable()
    {
        if(myQuest.isCompleted)
        {
            uncheckBox.sprite = checkedBox;
        }
    }
}
