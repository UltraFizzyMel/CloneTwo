using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // input fields
    private ThirdPersonActionsAsset playerActionAsset;
    private InputAction move;
    public BullActions bullActions;

    // movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    [Header("Camera Movement")]
    public CameraManager cameraManager;
    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;

    [Header("Bulldoze ability")]
    public GameObject bulldozer;
    public float chargeDuration = 5f;
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionsAsset();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
    }
    private void OnEnable()
    {
        playerActionAsset.Player.Jump.started += DoJump;
        move = playerActionAsset.Player.Move;
        playerActionAsset.Player.Enable();
        playerActionAsset.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
        playerActionAsset.Player.Effect.started += Charge;
    }
     
    private void OnDisable()
    {
        playerActionAsset.Player.Jump.canceled -= DoJump;
        playerActionAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCamaraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if(rb.angularVelocity.y < 0f)
        {
            rb.angularVelocity += Vector3.down * Physics.gravity.y * Time.fixedDeltaTime; /// the += here i think should be chaged to -=. Gravity Fix i think
        }

        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.linearVelocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;
        }

        LookAt();
        HandleCamera();
    }

    private void LookAt()
    {
        Vector3 direction = rb.linearVelocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCamaraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0f;
        return forward.normalized;
    }
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0f;
        return right.normalized;
    }
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            //forceDirection += Vector3.up * jumpForce;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.angularVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void HandleCamera()
    {
        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
    }

    public void Charge(InputAction.CallbackContext obj)
    {
        StartCoroutine(ChargeRoutine());
    }
    public System.Collections.IEnumerator ChargeRoutine()
    {
        // inceare bull running speed
        float originalSpeed = maxSpeed;

        maxSpeed = 10f;
        bulldozer.SetActive(true);
        yield return new WaitForSeconds(chargeDuration);
        bulldozer.SetActive(false);

        maxSpeed = originalSpeed;

        //reset bull speed
    }
}
