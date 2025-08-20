using UnityEngine;

public class ObjectHandling : MonoBehaviour
{
    public GameObject player;
    public GameObject playerObj;
    public PlayerMovement playerMovement;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the object if it collides with anything other than the enemy
            Destroy(gameObject);
        }
        else
        {
            // Stop all movement instantly
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;

            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            enemyAI.canEnemyMove = false;

            player.transform.SetParent(collision.transform, true);
            playerObj.SetActive(false);

            EnemyInfo enemyInfo = collision.gameObject.GetComponent<EnemyInfo>();
            playerMovement.enemyNum = enemyInfo.EnemyNum;
        }
    }
}
