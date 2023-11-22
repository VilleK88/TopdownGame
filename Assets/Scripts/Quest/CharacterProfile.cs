using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Profile", menuName = "Character Profile")]
public class CharacterProfile : ScriptableObject
{
    public string myName;
    public Sprite myPortrait;
    //Sprite myPortrait;
    public AudioClip myVoice;

    /*public Sprite MyPortrait
    {
        get
        {
            SetEmotionType(Emotion);
            return myPortrait;
        }
    }

    [System.Serializable]
    public class EmotionPortraits
    {
        public Sprite standard;
        public Sprite happy;
        public Sprite angry;
        public Sprite sad;
    }

    public EmotionPortraits emotionPortraits;

    public EmotionType Emotion { get; set; }

    public void SetEmotionType(EmotionType newEmotion)
    {
        Emotion = newEmotion;
        switch (Emotion)
        {
            case EmotionType.Standard:
                myPortrait = emotionPortraits.standard;
                break;
            case EmotionType.Happy:
                myPortrait = emotionPortraits.happy;
                break;
            case EmotionType.Angry:
                myPortrait = emotionPortraits.angry;
                break;
            case EmotionType.Sad:
                myPortrait = emotionPortraits.sad;
                break;
        }
    }*/
}

/*public enum EmotionType
{
    Standard,
    Happy,
    Angry,
    Sad
}*/
