using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerRailProgression : MonoBehaviour
{
    [SerializeField] float startSpeed = 5.0f;
    [SerializeField] float maxSpeed = 50.0f;
    [SerializeField] float acceleration = 1.0f;
    [SerializeField] CinemachineSplineCart dollyCart;

    float speed = 0;

    void Start()
    {
        speed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        dollyCart.SplinePosition += speed * Time.deltaTime / 1000f;

        if (speed < maxSpeed)
        {
            speed += Time.deltaTime * acceleration;
            speed = Mathf.Clamp(speed, 0, maxSpeed);
        }
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetSpeedRatio()
    {
        return speed / maxSpeed;
    }
}
