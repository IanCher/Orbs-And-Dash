using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DebugMode : MonoBehaviour
{
    [SerializeField] bool setPlayerSpeed = false;
    [SerializeField] bool isPlayerInvincible = false;
    [SerializeField] float playerSpeed = 30f;
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

        if (setPlayerSpeed) playerStats.currentSpeed = playerSpeed;
    }
}
