using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    enum LoopStyle { Classic, PushForFullLoop, Gravity };

    [SerializeField] LoopStyle loopStyle = LoopStyle.Classic;
    [SerializeField] private bool tieAngleToSpeed = false;
    [SerializeField] private float fixedMaxAngle = Mathf.PI / 2; 
    [SerializeField] float maxLateralSpeed = 2f;
    [SerializeField] float minLateralSpeed = 1f;
    [SerializeField] float playerOffset = 0;
    [SerializeField] float movementRadius = 5f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpControlPointOffset = 2f;
    [SerializeField] float timeBeforeFullLoop = 1.0f;
    [SerializeField] float fullLoopSpeed = 10f;
    [Space]
    [SerializeField] private bool enableSmoothing = true;
    [SerializeField] private float smoothingSpeed = 10f;

    float lateralSpeed = 2f;
    float moveDir = 0f;
    float maxAngle = 0f;
    float angle = 0;
    bool isJumping = false;
    float timeSincePlayerJumped = 0f;
    float timeSincePlayerPushesAtTheTop = 0f;
    bool isPushingAtTheTop = false;
    bool isGoingFullLoop = false;

    [SerializeField] float fallingAcceleration = 10f;
    bool isFalling = false;
    float targetAngle;
    float timeSinceFalling = 0f;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction muteAction;
    
    private PlayerStats playerStats;

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        muteAction = InputSystem.actions.FindAction("Mute");

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

        if (isFalling)
        {
            timeSinceFalling += Time.deltaTime;

            if (targetAngle > 0)
            {
                angle -= fallingAcceleration * timeSinceFalling * timeSinceFalling;
                angle = Mathf.Clamp(angle, targetAngle, angle);
            }
            else
            {
                angle += fallingAcceleration * timeSinceFalling * timeSinceFalling;
                angle = Mathf.Clamp(angle, angle, targetAngle);
            }

            transform.localPosition = new(
                transform.localPosition.x,
                GetPositionOnCircleFromAngle(angle).y,
                0
            );
            UpdateOrientation();

            if (Mathf.Abs(angle - targetAngle) < 1e-6)
            {
                isFalling = false;
                angle = targetAngle;
                UpdatePositionOnCircle();
                UpdateOrientation();
            }
            return;
        }

        ReadMoveInputAndUpdatePosition();

        if (isFalling) return;

        ReadJumpInput();
        ProcessJump();
        UpdateMaxAngle();
        UpdateLateralSpeed();
        ReadMuteInput();
    }

    void ReadMoveInputAndUpdatePosition()
    {
        switch (loopStyle)
        {
            case LoopStyle.Classic:
                ReadMoveInputAndUpdatePositionClassic();
                break;

            case LoopStyle.PushForFullLoop:
                ReadMoveInputAndUpdatePositionPushForFullLoop();
                break;

            case LoopStyle.Gravity:
                ReadMoveInputAndUpdatePositionClassic();
                break;

            default:
                return;
        }
    }

    void ReadMoveInputAndUpdatePositionClassic()
    {
        if (isJumping) return;

        float targetDir = movementAction.ReadValue<Vector2>().x;
        if(enableSmoothing)
        moveDir = Mathf.Lerp(moveDir, targetDir, Time.deltaTime * smoothingSpeed);
        else
            moveDir = targetDir;
            
        if (Mathf.Abs(moveDir) < 0.01)
        {
            if (loopStyle == LoopStyle.Gravity) FallDownIfTooHigh();
            return;
        }

        if (playerStats.Paralyzed)
        {
            moveDir= moveDir*0.1f;
        }
        angle += moveDir * lateralSpeed * Time.deltaTime;
        
        // Only clamp if we have any angle restriction
        if (tieAngleToSpeed)
        {
            if (maxAngle < Mathf.PI / 2)
            {
                angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
            }
        }

        UpdatePositionOnCircle();
        UpdateOrientation();
    }

    void FallDownIfTooHigh()
    {
        SetAngleInMinusPiPiRange();
        if (Mathf.Abs(angle) > Mathf.PI / 2)
        {
            targetAngle = Mathf.Sign(angle) * Mathf.PI - angle;
            timeSinceFalling = 0f;
            isFalling = true;
        }
    }


    void ReadMoveInputAndUpdatePositionPushForFullLoop()
    {
        if (isJumping) return;

        moveDir = movementAction.ReadValue<Vector2>().x;
        if (moveDir == 0f) {
            if (isPushingAtTheTop) isPushingAtTheTop = false;
            return;
        }

        angle += moveDir * lateralSpeed * Time.deltaTime;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        // Player is at the top
        if (Mathf.Abs(angle) == Mathf.PI / 2)
        {
            if (!isPushingAtTheTop)
            {
                isPushingAtTheTop = true;
                timeSincePlayerPushesAtTheTop = 0f;
            }

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
        if (!isJumping)
        {
            AudioManager.instance.PlaySound("Jump");
            playerStats.Jumped();
        }
            
            
        isJumping = true;
        
        // Make sure the player won't make more flips than necessary while jumping
        // by setting the angle in the range [-pi ; pi]
        SetAngleInMinusPiPiRange();
    }
    
    private void ReadMuteInput()
    {
        if (muteAction.IsPressed())
        {
            AudioListener.pause = !AudioListener.pause;
        }
    }

    private void SetAngleInMinusPiPiRange()
    {
        while (angle < -Mathf.PI)
        {
            angle += 2 * Mathf.PI;
        }
        while (angle > Mathf.PI)
        {
            angle -= 2 * Mathf.PI;
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
            (movementRadius - playerOffset) * Mathf.Sin(angle),
            -(movementRadius - playerOffset) * Mathf.Cos(angle) + playerOffset,
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
        maxAngle = tieAngleToSpeed ? 
            playerStats.GetSpeedRatio() * Mathf.PI / 2 : 
            fixedMaxAngle;
    }


    private void UpdateLateralSpeed()
    {
        if (lateralSpeed == maxLateralSpeed) return;
        float speedRatio = playerStats.GetSpeedRatio();
        lateralSpeed = Mathf.Lerp(minLateralSpeed, maxLateralSpeed, speedRatio);
    }

    private void ProcessFullLoopMovement()
    {
        angle += moveDir * fullLoopSpeed * Time.deltaTime;

        if (Mathf.Abs(angle) >= 1.52 * Mathf.PI)
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
            0,
            transform.position.z
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(movementSpherePosition, movementRadius);
    }
}
