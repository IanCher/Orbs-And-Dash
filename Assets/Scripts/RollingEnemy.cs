using UnityEngine;
using UnityEngine.SceneManagement;

public class RollingEnemy : MonoBehaviour, ICollideable
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
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime, Space.Self);
    }

    public void HandlePlayerCollision()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}