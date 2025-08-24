using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(PlayerStats))]
public class PlayerRailProgression : MonoBehaviour
{
    [SerializeField] CinemachineSplineCart dollyCart;
    [SerializeField] private bool enableLinearSpeedScaling = true;
    
    private PlayerStats playerStats;
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (enableLinearSpeedScaling)
        {
            playerStats.UpdateSpeed();
        }
        dollyCart.SplinePosition += playerStats.GetCurrentSpeed() * Time.deltaTime;
    }
}
