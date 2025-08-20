using UnityEngine;
using UnityEngine.AI;

public class NPCActions : MonoBehaviour
{
    public int CharacterNumber = 1;
    public Transform player;

    public bool isAwake = false;
    public float updateRate = 0.2f;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        isAwake = false;
    }

    private void OnEnable()
    {
        /*if(isAwake && player!= null)
        {
            StartCoroutine(FollowPlayer());
        }*/

        StartCoroutine(FollowPlayer());
    }

    private System.Collections.IEnumerator FollowPlayer()
    {
        while (isAwake && player != null)
        {
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(updateRate);
        }
    }

}
