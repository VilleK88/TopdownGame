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

    public GameObject levelTracker;
    [HideInInspector] public TextMeshProUGUI levelText;
    [HideInInspector] public Animator levelAnim;

    private void Start()
    {
        objectiveText.raycastTarget = false;
        levelText = levelTracker.GetComponent<TextMeshProUGUI>();
        levelAnim = levelTracker.GetComponent<Animator>();
        levelText.raycastTarget = false;
    }

    public void UpdateTracker(string newText)
    {
        objectiveText.text = newText;
        objectiveAnim.Play("ObjectivePopUp");
    }

    public void UpdateLevelTracker(string newText)
    {
        levelText.text = newText;
        levelAnim.Play("ObjectivePopUp");
    }
}
