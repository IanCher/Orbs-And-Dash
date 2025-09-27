using UnityEngine;

public static class PlayerData
{
    private const string NORMAL_ORBS = "NormalOrbs";
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
}
