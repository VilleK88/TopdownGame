using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    #region Singleton
    public static RewardManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public TextMeshProUGUI questName;
    public Image[] questRewardIcons;
    public TextMeshProUGUI questRewardXp;
    public GameObject questRewardUI;
    public QuestBase currentQuestReward;
    public bool isRewardUIActive;
    public bool inQuestReward { get; set; }

    public void SetRewardUI(QuestBase quest)
    {
        inQuestReward = true;

        questRewardUI.SetActive(true);

        questName.text = quest.questName;

        for(int i = 0; i < quest.rewards.itemRewards.Length; i++)
        {
            questRewardIcons[i].gameObject.SetActive(true);
            questRewardIcons[i].sprite = quest.rewards.itemRewards[i].icon;
        }

        questRewardXp.text = "Experience Reward: " + quest.rewards.experienceReward.ToString();
        currentQuestReward = quest;
        isRewardUIActive = true;
    }

    public void GetRewardButton()
    {
        GameManager.manager.currentExperience += currentQuestReward.rewards.experienceReward;

        QuestBase currentQuest = DialogueManager.instance.completedQuest;

        for (int i = 0; i < currentQuestReward.rewards.itemRewards.Length; i++)
        {
            bool wasPickedUp = InventoryManager.instance.AddItem(currentQuestReward.rewards.itemRewards[i]);
        }

        HasCompletedQuest(currentQuest);
        isRewardUIActive = false;
        StartCoroutine(QuestRewardBuffer());
    }

    public void HasCompletedQuest(QuestBase quest)
    {
        if (!GameManager.manager.completedQuests.Contains(quest))
        {
            AddQuestToCompletedArray(quest);
        }
    }

    public void AddQuestToCompletedArray(QuestBase quest)
    {
        QuestBase[] newQuestArray = new QuestBase[GameManager.manager.completedQuests.Length + 1];
        for (int i = 0; i < GameManager.manager.completedQuests.Length; i++)
        {
            newQuestArray[i] = GameManager.manager.completedQuests[i];
        }
        newQuestArray[GameManager.manager.completedQuests.Length] = quest;
        GameManager.manager.completedQuests = newQuestArray;
    }

    IEnumerator QuestRewardBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        inQuestReward = false;
    }
}
