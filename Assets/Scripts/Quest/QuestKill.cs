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

    void EnemyDeath(EnemyProfile slainEnemy)
    {
        for(int i = 0; i < objectives.Length; i++)
        {
            if(slainEnemy == objectives[i].requiredEnemy)
            {
                currentAmount[i]++;
            }
        }

        Evaluate();
    }
}
