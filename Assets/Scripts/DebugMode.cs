using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DebugMode : MonoBehaviour
{
    [SerializeField] bool setPlayerSpeedToMax = false;
    [SerializeField] bool isPlayerInvincible = false;
    [SerializeField] PlayerStats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (setPlayerSpeedToMax)
        {
            playerStats.currentSpeed = playerStats.MaxSpeed;
        }

        if (isPlayerInvincible)
        {
            foreach (PotionBomb potionBomb in FindObjectsByType<PotionBomb>(FindObjectsSortMode.None))
            {
                foreach (Collider collider in potionBomb.GetComponents<Collider>())
                {
                    collider.enabled = false;
                }
            }
            foreach (StaticWall staticWall in FindObjectsByType<StaticWall>(FindObjectsSortMode.None))
            {
                foreach (Collider collider in staticWall.GetComponents<Collider>())
                {
                    collider.enabled = false;
                }
            }
        }
    }

    void Update()
    {
        if (setPlayerSpeedToMax && (playerStats.currentSpeed < playerStats.MaxSpeed))
        {
            playerStats.currentSpeed = playerStats.MaxSpeed;
        }
    }
}
