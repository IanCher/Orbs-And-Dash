using UnityEngine;

public class ShopPlayerRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeedDegreesPerSecond = 45f;
    [SerializeField] private Vector3 localAxis = Vector3.up;   
    [SerializeField] private bool useUnscaledTime = true;      
    [SerializeField] private bool randomStartAngle = false;    

    private Vector3 _axis;

    private void Awake()
    {
        _axis = (localAxis == Vector3.zero ? Vector3.up : localAxis.normalized);
        if (randomStartAngle)
        {
            transform.Rotate(_axis, Random.Range(0f, 360f), Space.Self);
        }
    }

    private void Update()
    {
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        transform.Rotate(_axis, rotationSpeedDegreesPerSecond * dt, Space.Self);
    }
}