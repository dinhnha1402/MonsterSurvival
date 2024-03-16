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

            anim.SetBool("IsDeath", true);

            
            Destroy(gameObject, 1f);

        }
    }

}
