using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    #region Singleton
    public static ObjectiveTracker instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public TextMeshProUGUI objectiveText;
    public Animator objectiveAnim;

    private void Start()
    {
        objectiveText.raycastTarget = false;
    }

    public void UpdateTracker(string newText)
    {
        objectiveText.text = newText;
        objectiveAnim.Play("ObjectivePopUp");
    }
}
