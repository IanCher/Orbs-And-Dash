using UnityEngine;

public class RareOrb : MonoBehaviour , ICollideable
{

    public string id;

    public void HandlePlayerCollision()
    {
        PlayerData.RareOrbsTrack += id;
        PlayerData.RareOrbs++;
        gameObject.SetActive(false);
    }

}
