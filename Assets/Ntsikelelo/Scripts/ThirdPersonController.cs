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

    public float maxSpeed = 5f;
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

    public float captureRange = 3f;

    public GameObject player;
    //public ThirdPersonController playerThirdPersonController;

    public bool isEnemyView = false;

    [Header("Capture Mechanic")]
    public GameObject captureHatPrefab;
    public float forceAmount = 5f;
    public Transform shootPoint;

    [Header("Effects")]
    private EffectScript effectScript;

    public KeyCode throwKey = KeyCode.Mouse0;

    public PlayerMovement playerMovement;
    public GameObject playerObj;

    public CaptureManagement captureManagement;

    public Transform waypoint;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionsAsset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey))
        {
            /*EnemyAI enemyAI = gameObject.GetComponent<EnemyAI>();
            enemyAI.enabled = true;

            ThirdPersonController thirdPersonController = gameObject.GetComponent<ThirdPersonController>();
            thirdPersonController.enabled = false;
            playerMovement.enabled = true;

            //EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            //enemyAI.canEnemyMove = false;

            player.transform.SetParent(transform, false);
            playerObj.SetActive(true);*/
            captureManagement.ExitPossession(transform);
        }
    }

    private void LateUpdate()
    {
        //cameraManager.HandleAllCameraMovement();
    }
    private void OnEnable()
    {
        move = playerActionAsset.Player.Move;
        playerActionAsset.Player.Enable();
        //playerActionAsset.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
        playerActionAsset.Player.Effect.performed += UseEffect;
        //playerActionAsset.Player.Capture.started += NewCapture;
    }
     
    private void OnDisable()
    {
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

        //LookAt();
        //HandleCamera();
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
  
    public bool IsGrounded()
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

    private void UseEffect(InputAction.CallbackContext obj)
    {
        effectScript = GetComponent<EffectScript>();
        if(effectScript != null)
        {
            effectScript.DoEffect();
        }
    }

    /*public void CaptureMechanic()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, captureRange))
        {
            if (isEnemyView)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    cameraManager.targetTransform = hit.collider.transform;
                    hit.collider.GetComponent<ThirdPersonController>().enabled = true;
                    this.GetComponent<ThirdPersonController>().enabled = false;
                }
            }
            else
            {
                cameraManager.targetTransform = player.transform;
                this.GetComponent<ThirdPersonController>().enabled = false;
                playerThirdPersonController.enabled = true;

            }
        }
    }

    public void NewCapture(InputAction.CallbackContext obj)
    {
        Instantiate(captureHatPrefab, shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>().AddForce(shootPoint.forward * forceAmount, ForceMode.Impulse);
    }*/
}
