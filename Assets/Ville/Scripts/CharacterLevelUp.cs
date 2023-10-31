using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelUp : MonoBehaviour
{
    [SerializeField] float currentHealth, maxHealth, currentExperience, maxExperience, currentLevel;
    [SerializeField] TestPlayerHealth playerHealtScript;

    private void Start()
    {
        playerHealtScript = GetComponent<TestPlayerHealth>();
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

        playerHealtScript.maxHealth += 10;
        playerHealtScript.currentHealth = playerHealtScript.maxHealth;

        currentLevel++;

        currentExperience = 0;
        maxExperience += 100;
    }
}
