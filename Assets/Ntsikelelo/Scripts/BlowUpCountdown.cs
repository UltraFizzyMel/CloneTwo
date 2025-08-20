using UnityEngine;

public class BlowUpCountdown : MonoBehaviour
{
    public float countdownTime = 10f; // seconds
    public bool isCaptured = false;

    private float currentTime;

    void Start()
    {
        currentTime = countdownTime;
        StartCoroutine(Countdown());
    }

    private System.Collections.IEnumerator Countdown()
    {
        while (currentTime > 0)
        {
            if (!isCaptured)
            {
                currentTime -= Time.deltaTime;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
