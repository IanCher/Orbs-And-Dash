using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    enum MoveStyle { Rotational, Directional }; 
    // rotational = classic: [A] rotate clockwise, [D] rotate ccw
    // directional = new: [A] always go left, [D] always go right
    [SerializeField] MoveStyle moveStyle = MoveStyle.Rotational;

    enum LoopStyle { Classic, PushForFullLoop, Gravity };

    [SerializeField] LoopStyle loopStyle = LoopStyle.Classic;
    [SerializeField] private bool tieAngleToSpeed = false;
    [SerializeField] private float fixedMaxAngle = Mathf.PI / 2; 
    [SerializeField] float maxLateralSpeed = 2f;
    [SerializeField] float minLateralSpeed = 1f;
    [SerializeField] float playerOffset = 0;
    [SerializeField] float movementRadius = 5f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpDurationAtBottom = 1f;
    [SerializeField] float jumpControlPointOffset = 2f;
    [SerializeField] float bottomAngleLimit = 25f;
    [SerializeField] float timeBeforeFullLoop = 1.0f;
    [SerializeField] float fullLoopSpeed = 10f;
    [Space]
    [SerializeField] private bool enableSmoothing = true;
    [SerializeField] private float smoothingSpeed = 10f;
    [SerializeField] ParticleSystem[] broomVFX;

    float lateralSpeed = 2f;
    float moveDir = 0f;
    float maxAngle = 0f;
    float angle = 0;
    bool isJumpingToOtherSide = false;
    bool isJumpingAtBottom = false;
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

    public float defaultMovementRadius;
    [SerializeField] float bottomJumpHeight = 0.75f;

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string controlStyle = PlayerPrefs.GetString("ControlStyle", "default");
        if (controlStyle == "default")
        {
            moveStyle = MoveStyle.Directional;
            loopStyle = LoopStyle.Classic;
        }
        else
        {
            moveStyle = MoveStyle.Rotational;
            loopStyle = LoopStyle.Gravity;
        }

        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        muteAction = InputSystem.actions.FindAction("Mute");

        defaultMovementRadius = movementRadius;

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
            ProcessFall();
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

    private void ProcessFall()
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
        if (isJumpingToOtherSide) return;

        float targetDir = movementAction.ReadValue<Vector2>().x;

        // force angle to stay inside +- PI
        // otherwise we are dealing with >1 spin ie angles like 1080 degrees
        if (angle>Math.PI) angle -= Mathf.PI*2;
        if (angle<-Math.PI) angle += Mathf.PI*2;

        // new alternate movement: 
        // W moves to the top
        // A moves to left side
        // S moves to the bottom
        // D moves to right side
        if (moveStyle == MoveStyle.Directional)
        {
            bool aboveHalfway = ((angle <= -Math.PI/2) || (angle >= Math.PI/2));
            bool belowHalfway = !aboveHalfway;
            bool onLeftSide = (angle<0f);
            bool onRightSide = (angle>0f);
            bool goUp = movementAction.ReadValue<Vector2>().y>0f;
            bool goDown = movementAction.ReadValue<Vector2>().y<0f;
            bool goLeft = movementAction.ReadValue<Vector2>().x>0f;
            bool goRight = movementAction.ReadValue<Vector2>().x<0f;
            float cw = 1f;
            float ccw = -1f;
            if (goLeft && onLeftSide && aboveHalfway) targetDir = ccw;
            if (goLeft && onLeftSide && belowHalfway) targetDir = cw;
            if (goLeft && onRightSide && aboveHalfway) targetDir = ccw;
            if (goLeft && onRightSide && belowHalfway) targetDir = cw;
            if (goRight && onRightSide && aboveHalfway) targetDir = cw;
            if (goRight && onRightSide && belowHalfway) targetDir = ccw;
            if (goRight && onLeftSide && aboveHalfway) targetDir = cw;
            if (goRight && onLeftSide && belowHalfway) targetDir = ccw;
            if (goUp && onLeftSide) targetDir = ccw;
            if (goUp && onRightSide) targetDir = cw;
            if (goDown && onLeftSide) targetDir = cw;
            if (goDown && onRightSide) targetDir = ccw;
            /* Debug.Log("directional movement: "+
                (goLeft?"<":"-")+
                (goRight?">":"-")+
                (goUp?"^":"-")+
                (goDown?"v":"-")+
                (onLeftSide?"L":"-")+
                (onRightSide?"R":"-")+
                (aboveHalfway?"A":"-")+
                (belowHalfway?"B":"-")+
                " angle="+angle+
                " targetDir="+targetDir); */
        }  // end of new MoveStyle.Directional movement code

        if(enableSmoothing & !isJumpingAtBottom)
        {
            moveDir = Mathf.Lerp(moveDir, targetDir, Time.deltaTime * smoothingSpeed);
        }
        else
        {
            moveDir = targetDir;
        }
            
        if (Mathf.Abs(moveDir) < 0.01 & !isJumpingAtBottom)
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
        if (isJumpingToOtherSide) return;

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
        if (jumpAction.IsPressed() == IsJumping) return;
        if (IsJumping) return;
    
        AudioManager.instance.PlaySound("Jump");
        playerStats.Jumped();
        moveDir = 0f;

        // Make sure the player won't make more flips than necessary while jumping
        // by setting the angle in the range [-pi ; pi]
        SetAngleInMinusPiPiRange();

        foreach (ParticleSystem ps in broomVFX) ps.Stop();

        isJumpingAtBottom = IsAtBottomOfTrack() || playerStats.Paralyzed;
        isJumpingToOtherSide = !isJumpingAtBottom;
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
        if (!IsJumping) return;

        timeSincePlayerJumped += Time.deltaTime;
        if (IsJumpOver())
        {
            if (isJumpingToOtherSide)
            {
                angle = -angle;
                isJumpingToOtherSide = false;
            }
            else if (isJumpingAtBottom) isJumpingAtBottom = false;

            timeSincePlayerJumped = 0f;
            UpdatePositionOnCircle();
            UpdateOrientation();

            foreach (ParticleSystem ps in broomVFX) ps.Play();
        }
        else if (isJumpingAtBottom)
        {
            float param = timeSincePlayerJumped / jumpDurationAtBottom;

            float jumpHeight = playerStats.Paralyzed ? 0.1f * bottomJumpHeight : bottomJumpHeight;

            movementRadius = defaultMovementRadius * (
                1 - jumpHeight +
                jumpHeight * (4 * param * param - 4 * param + 1)
            );
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

    private bool IsAtBottomOfTrack()
    {
        return Mathf.Abs(angle * Mathf.Rad2Deg) < bottomAngleLimit;
    }

    private bool IsJumpOver()
    {
        if (isJumpingAtBottom) return timeSincePlayerJumped > jumpDurationAtBottom;
        return timeSincePlayerJumped > jumpDuration;
    }

    public bool IsJumping
    {
        get { return isJumpingAtBottom || isJumpingToOtherSide;}
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
