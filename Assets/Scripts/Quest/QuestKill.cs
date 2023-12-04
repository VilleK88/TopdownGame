using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kill Quest", menuName = "Quests/Kill Quest")]
public class QuestKill : QuestBase
{
    [System.Serializable]
    public class Objectives
    {
        public EnemyProfile requiredEnemy;
        public int requiredAmount;
    }

    public Objectives[] objectives;

    public override void initializeQuest()
    {
        requiredAmount = new int[objectives.Length];

        for(int i = 0; i < objectives.Length; i++)
        {
            requiredAmount[i] = objectives[i].requiredAmount;
        }

        GameManager.manager.onEnemyDeathCallBack += EnemyDeath;
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

    void EnemyDeath(EnemyProfile slainEnemy)
    {
        for(int i = 0; i < objectives.Length; i++)
        {
            if(slainEnemy == objectives[i].requiredEnemy)
            {
                currentAmount[i]++;
                ObjectiveTracker.instance.UpdateTracker($"You've slain {currentAmount[i] + "/" + requiredAmount[i] + " " + slainEnemy.enemyName}");
            }
        }

        Evaluate();
    }

    public override string GetObjectiveList()
    {
        string tempObjectiveList = "";

        for(int i = 0; i < objectives.Length; i++)
        {
            tempObjectiveList += $"You've slain {currentAmount[i]} / {requiredAmount[i]} {objectives[i].requiredEnemy.enemyName}s \n";
        }

        return tempObjectiveList;
    }

    public override string GetCompletedObjectiveList()
    {
        string completedObjectiveList = "";

        for(int i = 0; i < objectives.Length; i++)
        {
            completedObjectiveList += $"You've slain {requiredAmount[i]} / {requiredAmount[i]} {objectives[i].requiredEnemy.enemyName}s \n";
        }

        return completedObjectiveList;
    }
}
