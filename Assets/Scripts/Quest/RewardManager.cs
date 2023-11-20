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


    public void SetRewardUI(QuestBase quest)
    {
        questRewardUI.SetActive(true);

        questName.text = quest.questName;

        for(int i = 0; i < quest.rewards.itemRewards.Length; i++)
        {
            questRewardIcons[i].gameObject.SetActive(true);
            questRewardIcons[i].sprite = quest.rewards.itemRewards[i].icon;
        }

        currentQuestReward = quest;
    }

    public void GetRewardButton()
    {
        for(int i = 0; i < currentQuestReward.rewards.itemRewards.Length; i++)
        {
            bool wasPickedUp = InventoryManager.instance.AddItem(currentQuestReward.rewards.itemRewards[i]);
        }
    }
}
