using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DebugMode : MonoBehaviour
{
    [SerializeField] bool setPlayerSpeedToMax = false;
    [SerializeField] bool isPlayerInvincible = false;
    private PlayerStats playerStats;

    void Awake()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isPlayerInvincible)
        {
            playerStats.activeInvulnerabilityCount = 1;
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
