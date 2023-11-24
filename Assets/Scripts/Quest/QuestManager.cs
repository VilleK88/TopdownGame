using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager questManager;

    private void Awake()
    {
        if(questManager == null)
        {
            questManager = this;
        }
    }
    #endregion

    public GameObject questUI;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public Button questAcceptButton;

    public QuestBase currentQuest { get; set; }
    public QuestDialogueTrigger currentQuestDialogueTrigger { get; set; }
    public bool inQuestUI { get; set; }
    public int questIndex;


    public void SetQuestUI(QuestBase newQuest)
    {
        inQuestUI = true;
        currentQuest = newQuest;
        questUI.SetActive(true);
        questName.text = newQuest.questName;
        questDescription.text = newQuest.questDescription;
        questAcceptButton.Select();
    }

    public void StartQuestBuffer()
    {
        StartCoroutine(QuestBuffer());
    }

    IEnumerator QuestBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        inQuestUI = false;
    }

    /*public void MoveToFinishedQuests(QuestBase quest)
    {
        for(int i = 0; i < finishedQuests.Length; i++)
        {
            if (finishedQuests[i] == null)
            {
                finishedQuests[i] = quest;
                quests[i] = null;
                break;
            }
        }
    }*/
}
