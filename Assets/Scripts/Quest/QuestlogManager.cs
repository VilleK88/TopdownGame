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

    public void UpdateQuestlog(QuestBase newQuest)
    {
        questName.text = newQuest.questName;
        questDescription.text = newQuest.questDescription;
        //npcTurnInText.text = newQuest.NPCTurnIn.characterName;
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
        }
    }
}
