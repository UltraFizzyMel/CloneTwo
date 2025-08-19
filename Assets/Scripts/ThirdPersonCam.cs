using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public GameObject mainCam;

    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 camForward = transform.forward;
        camForward.y = 0f;                       // Flatten to ignore tilt
        if (camForward.sqrMagnitude > 0.001f)   // Prevent zero-length vector
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForward);
            orientation.rotation = Quaternion.Slerp(orientation.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        //rotate orientation
        //Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //orientation.forward = viewDir.normalized;

        //Vector3 cameraForward = mainCam.transform.forward;
        //cameraForward.y = 0; // Keep it flat
        //orientation.forward = cameraForward.normalized;

        //rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDir.normalized);
            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

        /*if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }*/

        /*if (inputDir.sqrMagnitude > 0.001f)
        {
            orientation.rotation = Quaternion.LookRotation(camForward.normalized);
        }*/
    }
}
