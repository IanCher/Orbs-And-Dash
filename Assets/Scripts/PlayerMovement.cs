using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float lateralSpeed = 2f;
    [SerializeField] float movementRadius = 10f;
    [SerializeField] float movementCenterHeight = 0f;

    float moveDir = 0f;
    float angle = 0;
    float radius;
    bool isInTheAir = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        angle = 0;
        radius = GetComponentInChildren<Renderer>().bounds.size.x / 2;
        isInTheAir = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDir == 0f) return;

        angle += moveDir * lateralSpeed * Time.deltaTime;
        UpdatePosition();
        UpdateOrientation();
    }

    void UpdatePosition()
    {
        transform.localPosition = new Vector3(
            (movementRadius - radius) * Mathf.Sin(angle),
            -(movementRadius - radius) * Mathf.Cos(angle) + movementRadius,
            0
        );
    }

    void UpdateOrientation()
    {
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg*angle);
    }

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        if (isInTheAir) return;

        isInTheAir = true;

    }

    private void OnDrawGizmos()
    {
        Vector3 movementSpherePosition = new Vector3(
            transform.position.x,
            movementCenterHeight,
            transform.position.z
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(movementSpherePosition, movementRadius);
   }
}
