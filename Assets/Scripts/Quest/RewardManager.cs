using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public TextMeshProUGUI questName;
    public Image[] questRewardIcons;
    public TextMeshProUGUI questRewardXp;
    public GameObject questRewardUI;

    public void SetRewardUI(QuestBase quest)
    {
        questRewardUI.SetActive(true);

        questName.text = quest.questName;

        for(int i = 0; i < quest.rewards.itemRewards.Length; i++)
        {
            questRewardIcons[i].gameObject.SetActive(true);
            questRewardIcons[i].sprite = quest.rewards.itemRewards[i].icon;
        }
    }
}
