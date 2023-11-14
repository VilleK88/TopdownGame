using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBase : ScriptableObject
{
    public string questName;
    [TextArea(5, 10)]
    public string questDescription;

    public int[] currentAmount { get; set; }
    public int[] requiredAmount { get; set; }

    public bool isCompleted { get; set; }

    public virtual void initializeQuest()
    {
        Debug.Log("Start Quest");
        currentAmount = new int[requiredAmount.Length];
    }

    public void Evaluate()
    {
        for(int i = 0; i < requiredAmount.Length; i++)
        {
            if (currentAmount[i] < requiredAmount[i])
            {
                return;
            }
        }

        Debug.Log("Quest is Completed");
    }
}
