using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    #region Singleton
    public static SkillTree instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public GameObject skillTreeUI;
    public bool isSkillTreeActive = false;
    public TextMeshProUGUI countText;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            skillTreeUI.SetActive(!skillTreeUI.activeSelf);
            isSkillTreeActive = skillTreeUI.activeSelf;
        }
    }

    public void RefreshSkillPointsCount()
    {
        countText.text = "Skill points: " + GameManager.manager.skillPoints;
    }
}
