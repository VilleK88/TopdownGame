using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    #region Singleton

    public static ExperienceManager instance;
    public delegate void ExperienceChangeHandler(float amount);
    public event ExperienceChangeHandler onExperienceChange;

    private void Awake()
    {
        //instance = this;

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

    public void AddExperience(float amount)
    {
        onExperienceChange?.Invoke(amount);
    }
}
