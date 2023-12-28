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

    public GameObject staminaRegenB;
    public GameObject staminaPotionPlusB;
    public GameObject staminaPlusFirstB;
    public GameObject staminaPlusSecondB;

    public Sprite staminaRegenSelected;
    public Sprite staminaPotionPlusSelected;
    public Sprite staminaPlusSelected;

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
            GameManager.manager.maxHealth += 25;
            GameManager.manager.currentHealth = GameManager.manager.maxHealth;
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
            GameManager.manager.maxHealth += 25;
            GameManager.manager.currentHealth = GameManager.manager.maxHealth;
        }
    }

    public void SelectStaminaRegen()
    {
        if (!GameManager.manager.staminaRegen && GameManager.manager.skillPoints > 0)
        {
            Image changeImage;
            changeImage = staminaRegenB.gameObject.GetComponent<Image>();
            changeImage.sprite = staminaRegenSelected;
            GameManager.manager.skillPoints--;
            RefreshSkillPointsCount();
            GameManager.manager.staminaRegen = true;
        }
    }

    public void SelectStaminaPotionPlus()
    {
        if (!GameManager.manager.staminaPotionPlus && GameManager.manager.skillPoints >= 2 &&
    GameManager.manager.staminaRegen)
        {
            Image changeImage;
            changeImage = staminaPotionPlusB.gameObject.GetComponent<Image>();
            changeImage.sprite = staminaPotionPlusSelected;
            GameManager.manager.skillPoints -= 2;
            RefreshSkillPointsCount();
            GameManager.manager.staminaPotionPlus = true;
        }
    }

    public void SelectStaminaPlusFirst()
    {
        if (!GameManager.manager.staminaPlusFirst && GameManager.manager.skillPoints >= 2 &&
    GameManager.manager.staminaPotionPlus)
        {
            Image changeImage;
            changeImage = staminaPlusFirstB.gameObject.GetComponent<Image>();
            changeImage.sprite = staminaPlusSelected;
            GameManager.manager.skillPoints -= 2;
            RefreshSkillPointsCount();
            GameManager.manager.staminaPlusFirst = true;
            GameManager.manager.maxStamina += 25;
            GameManager.manager.currentStamina = GameManager.manager.maxStamina;
        }
    }

    public void SelectStaminaPlusSecond()
    {
        if (!GameManager.manager.staminaPlusSecond && GameManager.manager.skillPoints >= 2 &&
GameManager.manager.staminaPlusFirst)
        {
            Image changeImage;
            changeImage = staminaPlusSecondB.gameObject.GetComponent<Image>();
            changeImage.sprite = staminaPlusSelected;
            GameManager.manager.skillPoints -= 2;
            RefreshSkillPointsCount();
            GameManager.manager.staminaPlusSecond = true;
            GameManager.manager.maxStamina += 25;
            GameManager.manager.currentStamina = GameManager.manager.maxStamina;
        }
    }
}
