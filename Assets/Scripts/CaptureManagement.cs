using Unity.Cinemachine;
using UnityEngine;

public class CaptureManagement : MonoBehaviour
{
    public GameObject player;
    public GameObject playerObj;
    public PlayerMovement playerMovement;
    private Collider[] playerCollider;
    private MeshRenderer[] meshRenderer;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = playerObj.GetComponentsInChildren<Collider>();
        meshRenderer = playerObj.GetComponentsInChildren<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            //EnemyAI enemyAI = gameObject.GetComponent<EnemyAI>();
            //enemyAI.enabled = false;

            ThirdPersonController thirdPersonController = gameObject.GetComponent<ThirdPersonController>();
            thirdPersonController.enabled = true;
            playerMovement.enabled = false;

            //EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            //enemyAI.canEnemyMove = false;

            player.transform.SetParent(transform, true);
            foreach (var mr in meshRenderer)
                mr.enabled = false;

            foreach (var col in playerCollider)
                col.enabled = false;
            rb.isKinematic = true;

            //EnemyInfo enemyInfo = collision.gameObject.GetComponent<EnemyInfo>();
            //playerMovement.enemyNum = enemyInfo.EnemyNum;
        }
    }

    public void ExitPossession(Transform enemy)
    {
        // Unparent from enemy
        player.transform.SetParent(null);
        ThirdPersonController thirdPersonController = enemy.GetComponent<ThirdPersonController>();
        player.transform.position = thirdPersonController.waypoint.position;

        // Re-enable visuals + physics
        foreach (var mr in meshRenderer)
            mr.enabled = true;

        foreach (var col in playerCollider)
            col.enabled = true;

        if (rb != null)
            rb.isKinematic = false;

        // Re-enable player movement
        player.GetComponent<PlayerMovement>().enabled = true;

        // Disable enemy movement script
        enemy.GetComponent<ThirdPersonController>().enabled = false;
    }
}
