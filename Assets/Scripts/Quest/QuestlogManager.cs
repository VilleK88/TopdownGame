using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestlogManager : MonoBehaviour
{
    #region Singleton
    public static QuestlogManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questName;
    public Transform questHolder;
    public GameObject questlogButtonPrefab;
    public GameObject questlogUI;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI npcTurnInText;
    public Image[] rewardIcons;
    QuestBase lastDisplayedQuest;

    public void UpdateQuestlogUI(QuestBase newQuest, string objectiveList)
    {
        lastDisplayedQuest = newQuest;
        questName.text = newQuest.questName;
        questDescription.text = newQuest.questDescription;
        npcTurnInText.text = "Turn into " + newQuest.NPCTurnIn.myName;
        objectiveText.text = objectiveList;
    }

    public void AddQuestLog(QuestBase newQuest)
    {
        var questButton = Instantiate(questlogButtonPrefab, questHolder);
        questButton.GetComponent<QuestlogButton>().SetQuest(newQuest);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            questlogUI.SetActive(!questlogUI.activeSelf);

            if(questlogUI.activeSelf)
            {
                //if (lastDisplayedQuest == null)
                    //return;

                try
                {
                    var firstButton = questHolder.GetChild(0).GetComponent<Button>();
                    firstButton.Select();
                    UpdateQuestlogUI(lastDisplayedQuest, lastDisplayedQuest.GetObjectiveList());
                }
                catch
                {
                    return;
                }

                //UpdateQuestlogUI(lastDisplayedQuest, lastDisplayedQuest.GetObjectiveList());
            }
        }
    }
}