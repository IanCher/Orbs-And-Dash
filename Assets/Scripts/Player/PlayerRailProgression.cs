using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(PlayerStats))]
public class PlayerRailProgression : MonoBehaviour
{
    [SerializeField] CinemachineSplineCart dollyCart;
    [SerializeField] FadeInOut fadeInOut;

    private PlayerStats playerStats;

    private bool isMoving = false;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    IEnumerator Start()
    {
        isMoving = false;
        yield return new WaitForSeconds(fadeInOut.GetFadeInTime() + 0.1f);
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) return;
        dollyCart.SplinePosition += playerStats.GetCurrentSpeed() * Time.deltaTime;
    }
}
