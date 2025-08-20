using UnityEngine;

public class Bulldozer : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BreakableWall"))
        {
            Debug.Log("Touched Wall");
            Destroy(other.gameObject);
        }
    }
}
