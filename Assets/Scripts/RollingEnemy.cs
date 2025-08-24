using UnityEngine;
using UnityEngine.SceneManagement;

public class RollingEnemy : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100;
    [SerializeField] private bool rotationClockwise = true;

    private Vector3 rotationDirection;

    private void Awake()
    {
        rotationDirection = rotationClockwise ? Vector3.left : Vector3.right;
    }
    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * rotationDirection, Space.Self);
    }
}