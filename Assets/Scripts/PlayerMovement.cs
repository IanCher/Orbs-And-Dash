using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float lateralSpeed = 2f;
    [SerializeField] float movementRadius = 10f;
    [SerializeField] float playerCenter = 0f;
    [SerializeField] float playerJumpSpeed = 1f;
    [SerializeField] float playerLandingSpeed = 1f;

    float moveDir = 0f;
    float angle = 0;
    float defaulPlayerCenter;
    bool isJumping = false;
    bool isLanding = false;

    InputAction movementAction;
    InputAction jumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        defaulPlayerCenter = playerCenter;

        UpdatePositionOnCircle();
        UpdateOrientation();
    }

    // Update is called once per frame
    void Update()
    {
        ReadMoveInputAndUpdatePosition();
        ReadJumpInput();
        ProcessJump();
    }

    void ReadMoveInputAndUpdatePosition()
    {
        moveDir = movementAction.ReadValue<Vector2>().x;
        if (moveDir == 0f) return;

        angle += moveDir * lateralSpeed * Time.deltaTime;
        UpdatePositionOnCircle();
        UpdateOrientation();
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

    void ReadJumpInput()
    {
        if (jumpAction.IsPressed() == isJumping) return;

        isJumping = jumpAction.IsPressed();
        if (!isJumping) isLanding = true;
    }

    void ProcessJump()
    {
        if (!isJumping && !isLanding) return;
        
        if (isJumping)
        {
            playerCenter += playerJumpSpeed * Time.deltaTime;
        }
        if (isLanding)
        {
            playerCenter -= playerJumpSpeed * Time.deltaTime;
            if (playerCenter <= defaulPlayerCenter)
            {
                playerCenter = defaulPlayerCenter;
                isLanding = false;
            }
        }
        UpdatePositionOnCircle();
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
