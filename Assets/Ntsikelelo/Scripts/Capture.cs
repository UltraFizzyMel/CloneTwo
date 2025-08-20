using UnityEngine;

public class Capture : MonoBehaviour
{
    public CameraManager cameraManager;
    public float destroyTime = 1f;
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            cameraManager.SetTarget(collision.gameObject.transform);
            Debug.Log("Hit capture target");
        }
    }
    private void Awake()
    {
        cameraManager = FindAnyObjectByType<CameraManager>();
        Destroy(this.gameObject, destroyTime);
    }
}
