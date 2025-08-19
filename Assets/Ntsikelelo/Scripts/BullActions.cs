using UnityEngine;
using UnityEngine.AI;

public class BullActions : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    public ThirdPersonController thirdPersonController;
    public CameraManager cameraManager;

    private NavMeshAgent agent;
    private bool isAwake = false;
    public float chargeDuration = 5f;
    public GameObject bulldozer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if (isAwake && player != null)
        {
            agent.SetDestination(player.position);
        }
        if (!isAwake)
        {
            StopChasing();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            WakeUp();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Sleep();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Captured();
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
        thirdPersonController.enabled = true;
        cameraManager.enabled = true;
    }

     public void Charge()
    {
        StartCoroutine(ChargeRoutine());
    }
    public System.Collections.IEnumerator ChargeRoutine()
    {
        /* inceare bull running speed
        float originalSpeed = maxSpeed;
        maxSpeed = 10f;
        */
        bulldozer.SetActive(true);
        yield return new WaitForSeconds(chargeDuration);
        bulldozer.SetActive(false);



        /*reset bull speed
        maxSpeed = originalSpeed;
        */
    }
}
