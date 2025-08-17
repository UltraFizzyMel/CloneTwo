using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    public Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCooldown;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode heldDashKey = KeyCode.LeftShift;
    public KeyCode tapDashKey = KeyCode.LeftControl;
    public KeyCode dashKey = KeyCode.E;

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        /*if (Input.GetKey(heldDashKey))
        {
            if (Input.GetKeyDown(tapDashKey))
                Dash();
        }*/

        if (Input.GetKeyDown(KeyCode.E))
        {
            Dash();
        }

        else if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (dashCdTimer > 0)
            return;
        else 
            dashCdTimer = dashCooldown;

        playerMovement.dashing = true;
        Debug.Log("Dashing");

        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        playerMovement.dashing = false;
    }
}
