using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerRailProgression : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] CinemachineSplineCart dollyCart;

    // Update is called once per frame
    void Update()
    {
        dollyCart.SplinePosition += speed * Time.deltaTime / 1000f;
    }
}
