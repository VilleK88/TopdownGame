using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

        isRewardUIActive = false;
        StartCoroutine(QuestRewardBuffer());
    }

    IEnumerator QuestRewardBuffer()
    {
        yield return new WaitForSeconds(0.1f);
        inQuestReward = false;
    }
}
