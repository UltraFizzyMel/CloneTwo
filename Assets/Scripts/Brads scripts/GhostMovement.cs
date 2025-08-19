using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float moveSpeed;

    //public float MaxFallSpeed = -2f;
    private Rigidbody rb;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        //if (rb.linearVelocity.y < MaxFallSpeed)
        //{
        //    rb.linearVelocity = new Vector3(rb.linearVelocity.x, MaxFallSpeed, rb.linearVelocity.z);
        //}

        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
