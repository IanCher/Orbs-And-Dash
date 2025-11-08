using UnityEngine;

public class RareOrb : MonoBehaviour , ICollideable
{

    public string id;
    private bool wasCollected = false;

    public void HandlePlayerCollision()
    {
        if (wasCollected) return;

        wasCollected = true;
        PlayerData.RareOrbsTrack += id;
        PlayerData.RareOrbs++;
        gameObject.SetActive(false);
    }

}
