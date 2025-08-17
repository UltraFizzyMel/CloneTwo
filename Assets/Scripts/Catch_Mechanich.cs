using UnityEngine;
using TMPro;
using System.Collections;

public class Catch_Mechanich : MonoBehaviour
{

    public float sphereRadius = 0.5f;   // "thickness" of the cast
    public float checkDistance = 2f;    // how far ahead to check
    public float launchSpeed = 20f;
    public LayerMask obstacleMask;      // what counts as an obstacle

    public GameObject projectile;
    private GameObject currentProjectile;

    public Transform firePoint; // empty GameObject in front of player

    public TextMeshProUGUI playerText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Left mouse or Ctrl
        {
            currentProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);

            // Apply velocity
            Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * launchSpeed;
            }
        }

        if (currentProjectile != null)
        {
            // Access its position while it's moving
            Vector3 projPos = currentProjectile.transform.position;
            Debug.Log("Projectile position: " + projPos);

            RaycastHit hit;
            Vector3 origin = projPos;
            Vector3 direction = currentProjectile.transform.forward;

            if (Physics.SphereCast(origin, sphereRadius, direction, out hit, checkDistance))
            {
                Debug.Log("Player is about to hit: " + hit.collider.name);

                if (hit.collider.CompareTag("Gost"))
                {
                    // Toggle off instead of destroy
                    hit.collider.gameObject.SetActive(false);

                    // Show message
                    //messageText.text = "Sphere disabled!";
                    StartCoroutine(ShowMessage("You collected a sphere!", 2f));
                    Debug.Log("Disabled: " + hit.collider.name);

                    Destroy(currentProjectile);
                }
            }
        }



    }

    IEnumerator ShowMessage(string text, float delay)
    {
        playerText.text = text;
        yield return new WaitForSeconds(delay);
        playerText.text = "";
    }

    void OnDrawGizmos()
    {
        // Debug visualisation
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.DrawWireSphere(transform.position + transform.forward * checkDistance, sphereRadius);
    }
}
