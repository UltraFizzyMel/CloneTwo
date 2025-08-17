using UnityEngine;

public class Move : MonoBehaviour
{

    Rigidbody capsule;

    Vector3 PanVector = Vector3.zero;

    Vector3 MoveVector;
    private Vector3 moveDirection;

    float MouseSpeed = 5;
    float Speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capsule = GetComponent<Rigidbody>();
        MoveVector = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //For Up and Down Rotation
        PanVector.z += Input.GetAxis("Mouse X") * MouseSpeed;
        //For Left and Right Rotation
        if (PanVector.x < 60 && PanVector.x > -50)
        {
            PanVector.x += Input.GetAxis("Mouse Y") * MouseSpeed;
        }
        else if (PanVector.x <= -50)
        {
            PanVector.x += 0.5f;
        }
        else if (PanVector.x >= 60)
        {
            PanVector.x -= 0.5f;
        }
        //Debug.Log(PanVector);
        Camera.main.transform.localRotation = Quaternion.Euler(-PanVector.x, 0, 0);
        transform.localRotation = Quaternion.Euler(0, PanVector.z, 0);
        // --- Movement input ---
        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S

        Vector3 input = new Vector3(h, 0, v);

        // Convert local input to world-space direction
        moveDirection = transform.TransformDirection(input) * Speed;

        // Apply movement
        transform.position += moveDirection * Time.deltaTime;
    }
}
