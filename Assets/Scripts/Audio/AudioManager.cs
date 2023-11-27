using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    #endregion

    public AudioSource audioSource;
    public AudioClip bgMusic;
    public AudioClip battleMusic;
    bool isPlayerDetected = false;

    //[Range(0f, 1f)]
    //public float backgroundVolume = 0.5f;


    private void Start()
    {
        //audioSource.loop = true;
        //PlayBackgroundMusic(bgMusic);
        //audioSource.volume = backgroundVolume;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if(audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
