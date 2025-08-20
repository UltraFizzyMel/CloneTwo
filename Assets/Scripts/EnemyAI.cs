using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;

    //Patrolling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    [SerializeField] private Transform[] Waypoints;

    //States
    public float sightRange;
    public bool playerInSightRange;

    public float attackRange;
    public bool playerInAttackRange;

    public bool canEnemyMove = true;
}
