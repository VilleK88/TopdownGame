using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject healthRegenB;
    public GameObject healthPotionPlusB;
    public GameObject healthPlusFirstB;
    public GameObject healthPlusSecondB;

    public Sprite healthRegenSelected;
    public Sprite healthPotionPlusSelected;
    public Sprite healthPlusSelected;

    private void Start()
    {
        RefreshSkillPointsCount();

        if (GameManager.manager.healthRegen)
        {
            Image changeImage;
            changeImage = healthRegenB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthRegenSelected;
        }
        if(GameManager.manager.healthPotionPlus)
        {
            Image changeImage;
            changeImage = healthPotionPlusB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthPotionPlusSelected;
        }
        if(GameManager.manager.healthPlusFirst)
        {
            Image changeImage;
            changeImage = healthPlusFirstB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthPlusSelected;
        }
        if (GameManager.manager.healthPlusSecond)
        {
            Image changeImage;
            changeImage = healthPlusSecondB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthPlusSelected;
        }
    }


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

    public void SelectHealthRegen()
    {
        if(!GameManager.manager.healthRegen && GameManager.manager.skillPoints > 0)
        {
            Image changeImage;
            changeImage = healthRegenB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthRegenSelected;
            GameManager.manager.skillPoints--;
            RefreshSkillPointsCount();
            GameManager.manager.healthRegen = true;
        }
    }

    public void SelectHealthPotionPlus()
    {
        if(!GameManager.manager.healthPotionPlus && GameManager.manager.skillPoints >= 2 &&
            GameManager.manager.healthRegen)
        {
            Image changeImage;
            changeImage = healthPotionPlusB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthPotionPlusSelected;
            GameManager.manager.skillPoints -= 2;
            RefreshSkillPointsCount();
            GameManager.manager.healthPotionPlus = true;
        }
    }

    public void SelectHealthPlusFirst()
    {
        if(!GameManager.manager.healthPlusFirst && GameManager.manager.skillPoints >= 2 &&
            GameManager.manager.healthPotionPlus)
        {
            Image changeImage;
            changeImage = healthPlusFirstB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthPlusSelected;
            GameManager.manager.skillPoints -= 2;
            RefreshSkillPointsCount();
            GameManager.manager.healthPlusFirst = true;
        }
    }

    public void SelectHealthPlusSecond()
    {
        if(!GameManager.manager.healthPlusSecond && GameManager.manager.skillPoints >= 2 &&
            GameManager.manager.healthPlusFirst)
        {
            Image changeImage;
            changeImage = healthPlusSecondB.gameObject.GetComponent<Image>();
            changeImage.sprite = healthPlusSelected;
            GameManager.manager.skillPoints -= 2;
            RefreshSkillPointsCount();
            GameManager.manager.healthPlusSecond = true;
        }
    }
}
