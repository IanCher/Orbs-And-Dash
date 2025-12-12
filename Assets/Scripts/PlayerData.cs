using UnityEngine;

public static class PlayerData
{
    public const string NORMAL_ORBS = "NormalOrbs";
    public const string RARE_ORBS = "RareOrbs";
    public const string RARE_ORBS_Track = "RareOrbsTrack";
    public const string TUTORIAL_COMPLETED = "TUTORIAL_COMPLETED";
    public const string OUTRO_COMPLETED = "OUTRO_COMPLETED";
    public const string INTRO_COMPLETED = "INTRO_COMPLETED";
    public static int NormalOrbs
    {
        get
        {
            return PlayerPrefs.GetInt(NORMAL_ORBS, 0);
        }
        set {
            PlayerPrefs.SetInt(NORMAL_ORBS, value);
        }
    }

    public static int RareOrbs
    {
        get
        {
            return PlayerPrefs.GetInt(RARE_ORBS, 0);
        }
        set
        {
            PlayerPrefs.SetInt(RARE_ORBS, value);
        }
    }

    public static int IsOutroComplete
    {
        get
        {
            return PlayerPrefs.GetInt(OUTRO_COMPLETED, 0);
        }
        set
        {
            PlayerPrefs.SetInt(OUTRO_COMPLETED, value);
        }
    }
    public static int IsIntroComplete
    {
        get
        {
            return PlayerPrefs.GetInt(INTRO_COMPLETED, 0);
        }
        set
        {
            PlayerPrefs.SetInt(INTRO_COMPLETED, value);
        }
    }



    public static string RareOrbsTrack
    {
        get
        {
            return PlayerPrefs.GetString(RARE_ORBS_Track, "");
        }
        set
        {
            PlayerPrefs.SetString(RARE_ORBS_Track, value);
        }
    }
    public static int TutorialCompleted
    {
        get
        {
            return PlayerPrefs.GetInt(TUTORIAL_COMPLETED, 0);
        }
        set
        {
            PlayerPrefs.SetInt(TUTORIAL_COMPLETED, value);
        }
    }
}
