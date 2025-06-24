using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float lateralSpeed = 2f;
    [SerializeField] float movementRadius = 10f;
    [SerializeField] float playerCenter = 0f;

    float moveDir = 0f;
    float angle = 0;
    bool isJumping = false;

    InputAction movementAction;
    InputAction jumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        UpdatePositionOnCircle();
        UpdateOrientation();
    }

    // Update is called once per frame
    void Update()
    {
        ReadMoveInputAndUpdateAngle();
        UpdatePositionOnCircle();
        UpdateOrientation();
        ReadAndProcessJumpInput();
    }

    void ReadMoveInputAndUpdateAngle()
    {
        moveDir = movementAction.ReadValue<Vector2>().x;
        if (moveDir == 0f) return;

        angle += moveDir * lateralSpeed * Time.deltaTime;
    }

    void UpdatePositionOnCircle()
    {
        transform.localPosition = new Vector3(
            (movementRadius - playerCenter) * Mathf.Sin(angle),
            -(movementRadius - playerCenter) * Mathf.Cos(angle) + movementRadius,
            0
        );
    }

    void UpdateOrientation()
    {
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg*angle);
    }

    void ReadAndProcessJumpInput()
    {
        if (jumpAction.IsPressed() && !isJumping)
        {
            print("I Jump");
            isJumping = true;
        }
        else if (!jumpAction.IsPressed() && isJumping)
        {
            print("I land");
            isJumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 movementSpherePosition = new Vector3(
            transform.position.x,
            movementRadius,
            transform.position.z
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(movementSpherePosition, movementRadius);
    }
}
