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

    public GameObject objectiveTrackerUI;
    public TextMeshProUGUI objectiveText;
    public Animator objectiveAnim;
    float maxTime = 3;
    float timer = 0;

    private void Start()
    {
        objectiveText = objectiveTrackerUI.GetComponent<TextMeshProUGUI>();
        objectiveAnim = objectiveTrackerUI.GetComponent<Animator>();
    }

    public void UpdateTracker(string newText)
    {
        timer = 0;
        objectiveTrackerUI.SetActive(true);
        objectiveText.text = newText;
        objectiveAnim.Play("ObjectivePopUp");

        if(timer < maxTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            objectiveTrackerUI.SetActive(false);
        }

    }
}
