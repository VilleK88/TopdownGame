using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLevelUp : MonoBehaviour
{
    public int currentLevel;
    public float currentExperience, maxExperience;
    TestPlayerHealth playerHealtScript;
    TestPlayer playerScript;

    float lerpTimer;
    float delayTimer;

    [Header("UI")]
    public Image frontXpBar;
    public Image backXpBar;

    [Header("Multipliers")]
    [Range(1, 300)]
    public float additionMultiplier = 300;
    [Range(2, 4)]
    public float powerMultiplier = 2;
    [Range(7, 14)]
    public float divisionMultiplier = 7;

    private void Start()
    {
        playerHealtScript = GetComponent<TestPlayerHealth>();
        playerScript = GetComponent<TestPlayer>();
        frontXpBar.fillAmount = currentExperience / maxExperience;
        backXpBar.fillAmount = currentExperience / maxExperience;
        //maxExperience = CalculateRequiredXp();
    }

    private void Update()
    {
        UpdateXpUI();
        if(Input.GetKeyDown(KeyCode.Equals))
        {
            GainExperienceFlatRate(20);
        }
        if(currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    public void OnEnable()
    {
        ExperienceManager.instance.onExperienceChange += HandleExperienceChange;
    }

    public void OnDisable()
    {
        ExperienceManager.instance.onExperienceChange -= HandleExperienceChange;
    }

    public void HandleExperienceChange(float newExperience)
    {
        currentExperience += newExperience;
        if(currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        //maxHealth += 10;
        //currentHealth = maxHealth;

        //playerHealtScript.maxHealth += 10;
        //playerHealtScript.currentHealth = playerHealtScript.maxHealth;

        currentLevel++;

        //currentExperience = 0;
        //maxExperience += 100;

        frontXpBar.fillAmount = 0;
        backXpBar.fillAmount = 0;
        currentExperience = Mathf.RoundToInt(currentExperience - maxExperience);
        playerHealtScript.IncreaseHealth(currentLevel);
        playerScript.IncreaseStamina(currentLevel);
        maxExperience = CalculateRequiredXp();
    }

    public void UpdateXpUI()
    {
        float xpFraction = currentExperience / maxExperience;
        float FXP = frontXpBar.fillAmount;
        if(FXP < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backXpBar.fillAmount = xpFraction;
            if(delayTimer > 1)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 2;
                frontXpBar.fillAmount = Mathf.Lerp(FXP, backXpBar.fillAmount, percentComplete);
            }
        }
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        currentExperience += xpGained;
        lerpTimer = 0;
        delayTimer = 0;
    }

    int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for(int levelCycle = 1; levelCycle <= currentLevel; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier,
                levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }
}
