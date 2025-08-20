using UnityEngine;
using UnityEngine.AI;

public class CameraManager : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;
    public NavMeshAgent agent;

    public Transform targetTransform;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public float cameraFollowSpeed = 0.2f;

    public float lookAngle; // up or down
    public float pivotAngle; // left or roght
    public float cameraLookSpeed = 2f;
    public float cameraPivotSpeed = 2f;

    public float minimumPivotAngle = -35f;
    public float maximumPivotAngle = 35f;
    public Transform cameraPivot;


    public void HandleAllCameraMovement()
    {
        RotateCamera();
        FollowTarget();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }
    private void RotateCamera()
    {
        lookAngle = lookAngle + (thirdPersonController.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (thirdPersonController.cameraInputY  * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

    }

    public void SetTarget(Transform newObject)
    {
        targetTransform.GetComponent<ThirdPersonController>().enabled = false; // turns off old target controller
        thirdPersonController = targetTransform.GetComponent<ThirdPersonController>();
        if (targetTransform.GetComponent<NPCActions>() != null) // checks if old had Ai controls
        {
            targetTransform.GetComponent<NPCActions>().enabled = true;
            targetTransform.GetComponent<NPCActions>().isAwake = true; // if so. Ai actions
        }
   
        targetTransform = newObject; // sets new target 

       
        targetTransform.GetComponent<ThirdPersonController>().enabled = true; // turn on new target controller
        thirdPersonController = targetTransform.GetComponent<ThirdPersonController>();
        if (targetTransform.GetComponent<NPCActions>() != null)
        {
            targetTransform.GetComponent<NPCActions>().isAwake = false;
            targetTransform.GetComponent<NPCActions>().enabled = false; //Disables AI actions
        }


    }
}
