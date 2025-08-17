using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    //private bool isSliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        //rb.freezeRotation = true; // Prevent the Rigidbody from rotating
        startYScale = playerObj.localScale.y; // Store the initial Y scale of the player
    }

    private void Update()
    {
        // Get input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        // Check if sliding
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            {
                StartSlide();
            }
            // Handle sliding state
            if (Input.GetKeyUp(slideKey) && playerMovement.sliding)
            {
                StopSlide();
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerMovement.sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        playerMovement.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement() 
    {
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(!playerMovement.OnSlope() || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        else
        {
            // If on a slope, move in the slope direction
            rb.AddForce(playerMovement.GetSlopeMoveDirection(inputDir) * slideForce, ForceMode.Force);
        }


        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        playerMovement.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
