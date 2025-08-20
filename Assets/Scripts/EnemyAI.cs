using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;

    //Patrolling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    [SerializeField] private Transform[] Waypoints;
    public Transform currentWaypoint;

    public LayerMask whatIsGround, whatIsPlayer;

    //States
    public float sightRange;
    public bool playerInSightRange;

    public float attackRange;
    public bool playerInAttackRange;

    public bool canEnemyMove = true;

    public Transform player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        //ChooseRandomWaypoint();
    }

    void Update()
    {
        if (canEnemyMove)
        {
            //agent.enabled = true;
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            //agent.isStopped = false;
            if (playerInSightRange)
            {
                agent.speed = 30f;
                agent.acceleration = 20f;
                agent.stoppingDistance = 10f;
                ChasePlayer();

                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
                if (playerInAttackRange)
                {
                    //DAMAGE PLAYER HERE
                }
            }
            else if (!playerInSightRange)
            {
                //Change to Patrol Speed and Acceleration
                agent.speed = 10f;
                agent.acceleration = 5f;
                agent.stoppingDistance = 0f;
                //Patrol();
            }
        }
    }

    private void Patrol()
    {
        // Move towards the current waypoint
        agent.SetDestination(currentWaypoint.position);
        //Debug.Log("Moving to waypoint: " + currentWaypoint.position);

        // Check if the enemy has reached the waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 5f)
        {
            Debug.Log("Reached waypoint: " + currentWaypoint.name);
            ChooseRandomWaypoint(); // Choose a new waypoint once the current one is reached
        }
    }

    private void ChooseRandomWaypoint()
    {
        int randomIndex = UnityEngine.Random.Range(0, Waypoints.Length);
        currentWaypoint = Waypoints[randomIndex];
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
}
