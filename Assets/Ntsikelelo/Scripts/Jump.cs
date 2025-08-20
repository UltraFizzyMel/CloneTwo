using UnityEngine;

public class Jump : MonoBehaviour
{
    ThirdPersonController thirdPersonController;
    private Rigidbody rb;
    public float jumpForce;
    public void OnAwake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        rb = GetComponent<Rigidbody>();
    }

    public void JumpMethod()
    {
        if (thirdPersonController.IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
