using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float currentHealth, maxHealth, currentExperience, maxExperience, currentLevel;

    private void OnEnable()
    {
        ExperienceManager.instance.onExperienceChange += HandleExperienceChange;
    }

    private void OnDisable()
    {
        ExperienceManager.instance.onExperienceChange -= HandleExperienceChange;
    }

    void HandleExperienceChange(float newExperience)
    {
        currentExperience += newExperience;
        if(currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        maxHealth += 10;
        currentHealth = maxHealth;

        currentLevel++;

        currentExperience = 0;
        maxExperience += 100;
    }
}
