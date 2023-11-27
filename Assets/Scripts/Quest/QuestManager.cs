using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

    [SerializeField] AudioClip questStartsSound;


    private void Start()
    {
        if (GameManager.manager != null && GameManager.manager.triggeredQuests != null)
        {
            foreach(QuestBase quest in GameManager.manager.triggeredQuests)
            {
                quest.initializeQuest();
            }
        }
    }


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

    public void QuestAcceptedSound()
    {
        AudioManager.instance.PlaySound(QuestManager.questManager.questStartsSound);
    }

    public void QuestAlreadyAccepted()
    {

    }

    public void HasTriggeredQuest(QuestBase quest)
    {
        AddQuestToArray(quest);
    }

    public void HasCompletedQuest(QuestBase quest)
    {
        GameManager.manager.completedQuests.Contains(quest);
    }

    void AddQuestToArray(QuestBase quest)
    {
        QuestBase[] newQuestArray = new QuestBase[GameManager.manager.triggeredQuests.Length + 1];
        for (int i = 0; i < GameManager.manager.triggeredQuests.Length; i++)
        {
            newQuestArray[i] = GameManager.manager.triggeredQuests[i];
        }
        newQuestArray[GameManager.manager.triggeredQuests.Length] = quest;
        GameManager.manager.triggeredQuests = newQuestArray;
    }
}
