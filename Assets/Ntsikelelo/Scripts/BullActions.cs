using UnityEngine;
using UnityEngine.AI;

public class BullActions : MonoBehaviour
{
    public Transform player;
    public ThirdPersonActionsAsset thirdPersonController;
    private NavMeshAgent agent;
    private bool isAwake = false;
    public float chargeDuration = 5f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Sleep();
    }

    public void Update()
    {
        if (isAwake && player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void StartChasing()
    {
        isAwake = true;
    }
    public void StopChasing()
    {
        isAwake = false;
        agent.ResetPath();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // call damage player method
        }
    }

    public void Sleep()
    {
        isAwake = false;
    }
    public void WakeUp()
    {
        isAwake = true;
       // disable controller
    }
    public void Captured()
    {
        isAwake = false;
      // enable controller
    }
}
