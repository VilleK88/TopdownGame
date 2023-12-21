using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save Quest", menuName = "Quests/Save Quest")]
public class QuestSave : QuestBase
{
    [System.Serializable]
    public class Objectives
    {
        public CharacterProfile requiredCharacter;
        public int requiredAmount;
    }

    public Objectives[] objectives;

    public override void initializeQuest()
    {
        requiredAmount = new int[objectives.Length];

        for (int i = 0; i < objectives.Length; i++)
        {
            requiredAmount[i] = objectives[i].requiredAmount;
        }

        GameManager.manager.onCharacterSaveCallBack += CharacterSaved;
        base.initializeQuest();
    }

    public override void InitializeRewardReadyQuest()
    {
        requiredAmount = new int[objectives.Length];

        for (int i = 0; i < objectives.Length; i++)
        {
            requiredAmount[i] = objectives[i].requiredAmount;
        }

        base.InitializeRewardReadyQuest();
    }

    public override void InitializeCompletedQuest()
    {
        requiredAmount = new int[objectives.Length];

        for (int i = 0; i < objectives.Length; i++)
        {
            requiredAmount[i] = objectives[i].requiredAmount;
        }

        base.InitializeCompletedQuest();
    }

    void CharacterSaved(CharacterProfile savedCharacter)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (savedCharacter == objectives[i].requiredCharacter)
            {
                currentAmount[i]++;
                ObjectiveTracker.instance.UpdateTracker($"You've saved {currentAmount[i] + "/" + requiredAmount[i] + " " + savedCharacter.myName}");
            }
        }

        Evaluate();
    }

    public override string GetObjectiveList()
    {
        string tempObjectiveList = "";

        for (int i = 0; i < objectives.Length; i++)
        {
            tempObjectiveList += $"You've saved {currentAmount[i]} / {requiredAmount[i]} {objectives[i].requiredCharacter.myName}s \n";
        }

        return tempObjectiveList;
    }

    public override string GetCompletedObjectiveList()
    {
        string completedObjectiveList = "";

        for (int i = 0; i < objectives.Length; i++)
        {
            completedObjectiveList += $"You've saved {requiredAmount[i]} / {requiredAmount[i]} {objectives[i].requiredCharacter.myName}s \n";
        }

        return completedObjectiveList;
    }
}
