using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float maxLateralSpeed = 2f;
    [SerializeField] float minLateralSpeed = 1f;
    [SerializeField] float movementRadius = 10f;
    [SerializeField] float playerCenter = 0f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpControlPointOffset = 2f;
    [SerializeField] float timeBeforeFullLoop = 1.0f;
    [SerializeField] float fullLoopSpeed = 10f;

    float lateralSpeed = 2f;
    float moveDir = 0f;
    float maxAngle = 0f;
    float angle = 0;
    bool isJumping = false;
    float timeSincePlayerJumped = 0f;
    float timeSincePlayerPushesAtTheTop = 0f;
    bool isPushingAtTheTop = false;
    bool isGoingFullLoop = false;

    InputAction movementAction;
    InputAction jumpAction;

    PlayerRailProgression railProgression;


    void Awake()
    {
        railProgression = GetComponent<PlayerRailProgression>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        UpdatePositionOnCircle();
        UpdateOrientation();
        UpdateMaxAngle();
        UpdateLateralSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoingFullLoop)
        {
            ProcessFullLoopMovement();
            return;
        }

        ReadMoveInputAndUpdatePosition();
        ReadJumpInput();
        ProcessJump();
        UpdateMaxAngle();
        UpdateLateralSpeed();
    }

    void ReadMoveInputAndUpdatePosition()
    {
        if (isJumping) return;

        moveDir = movementAction.ReadValue<Vector2>().x;
        if (moveDir == 0f) return;

        angle += moveDir * lateralSpeed * Time.deltaTime;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        // Player is at the top
        if ((Mathf.Abs(angle) == Mathf.PI / 2) && !isPushingAtTheTop)
        {
            isPushingAtTheTop = true;
            timeSincePlayerPushesAtTheTop = 0f;
        }
        else if (isPushingAtTheTop)
        {
            timeSincePlayerPushesAtTheTop += Time.deltaTime;
            if (timeSincePlayerPushesAtTheTop >= timeBeforeFullLoop)
            {
                isGoingFullLoop = true;
                isPushingAtTheTop = false;
            }
        }

        UpdatePositionOnCircle();
        UpdateOrientation();
    }

    void UpdatePositionOnCircle()
    {
        transform.localPosition = GetPositionOnCircleFromAngle(angle);
    }

    void UpdateOrientation()
    {
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle);
    }

    void ReadJumpInput()
    {
        if (jumpAction.IsPressed() == isJumping) return;
        isJumping = true;

        // Make sure the player won't make more flips than necessary while jumping
        // by setting the angle in the range [-pi ; pi]
        SetAngleInMinusPiPiRange();
    }

    private void SetAngleInMinusPiPiRange()
    {
        while (angle < -Mathf.PI)
        {
            angle += 2 * Mathf.PI;
        }
        while (angle > Mathf.PI)
        {
            angle -= 2*Mathf.PI;
        }
    }

    void ProcessJump()
    {
        if (!isJumping) return;

        timeSincePlayerJumped += Time.deltaTime;
        if (timeSincePlayerJumped > jumpDuration)
        {
            isJumping = false;
            angle = -angle;
            timeSincePlayerJumped = 0f;
            UpdatePositionOnCircle();
            UpdateOrientation();
        }
        else
        {
            float param = timeSincePlayerJumped / jumpDuration;
            transform.localPosition = BezierCurve(param);
            transform.localEulerAngles = new Vector3(
                0,
                0,
                Mathf.Rad2Deg * Mathf.Lerp(angle, -angle + 4 * Mathf.Sign(angle) * Mathf.PI, param)
            );
        }
    }

    private Vector3 GetPositionOnCircleFromAngle(float angle)
    {
        return new Vector3(
            (movementRadius - playerCenter) * Mathf.Sin(angle),
            -(movementRadius - playerCenter) * Mathf.Cos(angle) + movementRadius,
            0
        );
    }

    private Vector3 BezierCurve(float param)
    {
        Vector3 startPoint = GetPositionOnCircleFromAngle(angle);
        Vector3 endPoint = GetPositionOnCircleFromAngle(-angle);
        Vector3 controlPoint = new Vector3(
            0,
            transform.localPosition.y + jumpControlPointOffset, 
            0
        );

        Vector3 start2control = Vector3.Lerp(startPoint, controlPoint, param);
        Vector3 control2end = Vector3.Lerp(controlPoint, endPoint, param);

        return Vector3.Lerp(start2control, control2end, param);
    }

    private void UpdateMaxAngle()
    {
        if (angle == Mathf.PI / 2) return;
        maxAngle = railProgression.GetSpeedRatio() * Mathf.PI / 2;
    }

    private void UpdateLateralSpeed()
    {
        if (lateralSpeed == maxLateralSpeed) return;
        lateralSpeed = maxLateralSpeed * railProgression.GetSpeedRatio();
        lateralSpeed = Mathf.Clamp(lateralSpeed, minLateralSpeed, maxLateralSpeed);
    }

    private void ProcessFullLoopMovement()
    {
        angle += moveDir * fullLoopSpeed * Time.deltaTime;

        if (Mathf.Abs(angle) >= 1.5 * Mathf.PI)
        {
            isGoingFullLoop = false;
            SetAngleInMinusPiPiRange();
        }

        UpdatePositionOnCircle();
        UpdateOrientation();
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
