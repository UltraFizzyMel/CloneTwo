using UnityEngine;

public class EffectScript : MonoBehaviour
{
    public int CharacterNumber = 1;
    ThirdPersonController thirdPersonController;

    [Header("Bull Settings")]
    public GameObject bulldozer;
    public float chargeSpeed = 10f;
    public float chargeDuration = 5f;
    public ParticleSystem chargeSmoke;


    [Header("Bullet Settings")]
    public float something = 0f;
    public float AccelerateDuration = 3f;
    public float AccelerateSpeed = 12f;
    public ParticleSystem BulletSmoke;

    [Header("Ghost Settings")]
    public float somethingElse = 0f;

    private void Start()
    {
        if(BulletSmoke != null)
        {
            BulletSmoke.Stop();
        }
        if (chargeSmoke != null)
        {
            chargeSmoke.Stop();
        }
    }
    public void DoEffect()
    {
        if (CharacterNumber == 1) // Player 
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
        }
        if (CharacterNumber == 2) // Bull
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            Charge();
        }
        if (CharacterNumber == 3) // Bullet
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            Accelerate();
        }
        if (CharacterNumber == 4) // Ghost
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
        }
        else
        {
            return;
        }
    }

    public void Charge()
    {
        StartCoroutine(ChargeRoutine());
    }
    public System.Collections.IEnumerator ChargeRoutine()
    {
        float originalSpeed = thirdPersonController.maxSpeed;
        thirdPersonController.maxSpeed = chargeSpeed;

        bulldozer.SetActive(true);
        chargeSmoke.Play();
        yield return new WaitForSeconds(chargeDuration);
        bulldozer.SetActive(false);
        chargeSmoke.Stop();
        thirdPersonController.maxSpeed = originalSpeed;
    }

    public void Accelerate()
    {
       StartCoroutine (AccelerateRoutine());
    }
    public System.Collections.IEnumerator AccelerateRoutine()
    {
        float originalSpeed = thirdPersonController.maxSpeed;
        thirdPersonController.maxSpeed = AccelerateSpeed;
        BulletSmoke.Play();
        yield return new WaitForSeconds(AccelerateDuration);
        BulletSmoke.Stop();
        thirdPersonController.maxSpeed = originalSpeed;
    }
}
