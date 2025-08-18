using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;



public class Damage_System : MonoBehaviour
{

    public int Health = 100;
    float value;
    bool ShouldDeduct = true;

    float velocity = 0f;

    float smoothTime = 1f;

    public TextMeshProUGUI Health_text;

    public Slider HealthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthBar.value = Health;
    }

    // Update is called once per frame
    void Update()
    {
        //HealthBar.value = Health;
        value = Mathf.SmoothDamp(value, Health, ref velocity, smoothTime);
        HealthBar.value = value;
        Health_text.text = Health.ToString();
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Gost" && ShouldDeduct)
        {
            Health--;
            ShouldDeduct = false;
            StartCoroutine(DelayHealth(0.2f));
        }
    }

    IEnumerator DelayHealth(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShouldDeduct = true;
    }
}
