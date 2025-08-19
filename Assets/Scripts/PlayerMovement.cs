using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;

    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float maxYSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    bool isJumping = false;
    [SerializeField] private int maxJumps = 2;  // 2 = double jump
    private int jumpCount = 0;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        dashing,
        air
    }

    public bool dashing;
    public bool sliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent the Rigidbody from rotating

        startYScale = transform.localScale.y; // Store the initial Y scale of the player
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        Myinput();
        SpeedControl();
        StateHandler();

        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
        {
            rb.linearDamping = groundDrag; // Apply ground drag when grounded
        }
        else if (!grounded || state == MovementState.dashing || state == MovementState.sliding)
        {
            rb.linearDamping = 0;
        }

        if (grounded && jumpCount > 0)
        {
            jumpCount = 0; // Reset jump count when grounded
            readyToJump = true; // Reset jump readiness
            Invoke(nameof(ResetJump), jumpCooldown); // Reset jump after cooldown
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Myinput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        /*if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // Reset jump after cooldown
        }
        else if (Input.GetKeyDown(jumpKey) && isJumping == true)
        {
            readyToJump = false;
            Jump();
        }*/

        if (Input.GetKeyDown(jumpKey) && readyToJump)
        {
            if (grounded || jumpCount < maxJumps)
            {
                Jump();
            }
        }

        else if (Input.GetKeyDown(crouchKey))
        {
            // Crouch
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            //moveSpeed = crouchSpeed; // Set crouch speed
        }
        else if (Input.GetKeyUp(crouchKey))
        {
            // Stop crouching
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            //moveSpeed = walkSpeed; // Reset to walk speed
        }
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpDashSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed; // Ensure final value is set
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private MovementState lastState;
    private bool keepMomentum;
    private void StateHandler()
    {
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.angularVelocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed; // Use sliding speed on slopes
            else
                desiredMoveSpeed = sprintSpeed; // Set sliding speed
        }

        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed; // Set crouch speed
        }

        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed; // Default to walk speed in air
            else
                desiredMoveSpeed = sprintSpeed; // Air speed can be adjusted
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            // Smoothly lerp the move speed if it has changed significantly
            StopCoroutine(SmoothlyLerpMoveSpeed());
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            // Directly set the move speed if the change is small
            moveSpeed = desiredMoveSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopCoroutine(SmoothlyLerpDashSpeed());
                StartCoroutine(SmoothlyLerpDashSpeed());
            }
            else
            {
                StopCoroutine(SmoothlyLerpDashSpeed());
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed; // Ensure final value is set
    }

    private void MovePlayer()
    {
        if (state == MovementState.dashing || state == MovementState.sliding) return;
        
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            // If on a slope, move in the slope direction
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            
            if (rb.linearVelocity.y < 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force); // Prevent sliding down slopes
            }
        }

        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();

        //moveDirection.Normalize(); // Normalize to ensure consistent speed in all directions
        // Apply the movement to the Rigidbody
        //rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if(rb.linearVelocity.magnitude > moveSpeed)
            {
                //Vector3 limitedVel = rb.linearVelocity.normalized * moveSpeed;
                //rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }     
        
        if (maxYSpeed != 0 && rb.linearVelocity.y > maxYSpeed)
        {
            // Limit the vertical speed
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z);
        }
    }

    private void Jump()
    {
        jumpCount++;
        readyToJump = true;

        isJumping = true;

        exitingSlope = true; // Set exiting slope to true to handle slope jumping

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset vertical velocity before jumping

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isJumping = false; // Reset jumping state

        readyToJump = true; // Reset the jump cooldown

        exitingSlope = false; // Reset exiting slope state
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}