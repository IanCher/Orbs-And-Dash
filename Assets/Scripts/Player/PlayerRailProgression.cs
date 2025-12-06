using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(PlayerStats))]
public class PlayerRailProgression : MonoBehaviour
{
    [SerializeField] CinemachineSplineCart dollyCart;
    [SerializeField] FadeInOut fadeInOut;
    [SerializeField] TimerUI timerUI;

    private PlayerStats playerStats;

    private bool isMoving = false;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        StartCounter.OnGo += StartCounter_OnGo;
    }

    private void OnDestroy()
    {
        StartCounter.OnGo -= StartCounter_OnGo;
    }

    private void StartCounter_OnGo()
    {
        isMoving = true;
        timerUI.StartTrackingTime();
    }

    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) return;
        dollyCart.SplinePosition += playerStats.GetCurrentSpeed() * Time.deltaTime;
    }
}
