using UnityEngine;

public static class PlayerData
{
    public const string NORMAL_ORBS = "NormalOrbs";
    public const string RARE_ORBS = "RareOrbs";
    public const string RARE_ORBS_Track = "RareOrbsTrack";
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
}
