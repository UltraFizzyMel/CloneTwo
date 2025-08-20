using UnityEngine;

public class Move : MonoBehaviour
{

    Rigidbody capsule;

    Vector3 PanVector = Vector3.zero;

    Vector3 MoveVector;

    float MouseSpeed = 5;
    float Speed = 0.1f;
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
        if (PanVector.z < 50 && PanVector.z > -50)
        {
            PanVector.z += Input.GetAxis("Mouse X") * MouseSpeed;
        }
        else if (PanVector.z >= 50)
        {
            PanVector.z -= 0.5f;
        }
        else if (PanVector.z <= -50)
        {
            PanVector.z += 0.5f;
        }
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
        Camera.main.transform.localRotation = Quaternion.Euler(-PanVector.x, PanVector.z, 0);

        if (Input.GetAxis("Vertical") != 0)
        {
            MoveVector.z += Input.GetAxis("Vertical") * Speed;

        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            MoveVector.x += Input.GetAxis("Horizontal") * Speed;
        }

        transform.position = MoveVector;
    }
}
