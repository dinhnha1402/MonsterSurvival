using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{

    public static PlayerHealthController Instance;

    public float currentHealth, maxHealth;
    public Animator anim;

    public Slider healthSlider;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            StartCoroutine(AnimDeath());
        }
    }

    IEnumerator AnimDeath()
    {
        anim.SetBool("IsDeath", true);

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

}
