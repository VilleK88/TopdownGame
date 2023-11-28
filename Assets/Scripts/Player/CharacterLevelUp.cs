using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLevelUp : MonoBehaviour
{
    PlayerHealth playerHealtScript;
    Player playerScript;

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
        playerHealtScript = GetComponent<PlayerHealth>();
        playerScript = GetComponent<Player>();
        frontXpBar.fillAmount = GameManager.manager.currentExperience / GameManager.manager.maxExperience;
        backXpBar.fillAmount = GameManager.manager.currentExperience / GameManager.manager.maxExperience;
    }

    private void Update()
    {
        UpdateXpUI();
        if(Input.GetKeyDown(KeyCode.Equals))
        {
            GainExperienceFlatRate(20);
        }
        if(GameManager.manager.currentExperience >= GameManager.manager.maxExperience)
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
        GameManager.manager.currentExperience += newExperience;
        if(GameManager.manager.currentExperience >= GameManager.manager.maxExperience)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        GameManager.manager.currentLevel++;
        ObjectiveTracker.instance.UpdateLevelTracker($"Level increased to {GameManager.manager.currentLevel}");

        frontXpBar.fillAmount = 0;
        backXpBar.fillAmount = 0;
        GameManager.manager.currentExperience = Mathf.RoundToInt(GameManager.manager.currentExperience - GameManager.manager.maxExperience);
        playerHealtScript.IncreaseHealth(GameManager.manager.currentLevel);
        playerScript.IncreaseStamina(GameManager.manager.currentLevel);
        GameManager.manager.maxExperience = CalculateRequiredXp();
    }

    public void UpdateXpUI()
    {
        float xpFraction = GameManager.manager.currentExperience / GameManager.manager.maxExperience;
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
        GameManager.manager.currentExperience += xpGained;
        lerpTimer = 0;
        delayTimer = 0;
    }

    int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for(int levelCycle = 1; levelCycle <= GameManager.manager.currentLevel; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier,
                levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }
}
