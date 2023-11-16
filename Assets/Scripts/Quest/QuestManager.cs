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

    public void SetQuestUI(QuestBase newQuest)
    {
        currentQuest = newQuest;
        questUI.SetActive(true);
        questName.text = newQuest.questName;
        questDescription.text = newQuest.questDescription;
    }
}
