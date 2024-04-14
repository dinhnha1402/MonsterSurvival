using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRb;

    private GameObject target;

    public Animator anim;

    public SpriteRenderer spriteRenderer;

    public float moveSpeed;

    public float damage;

    public float attackRange = 0f;

    public float hitDelayTime = 1f;
    private float hitDelayTimeCounter;

    private float distanceToPlayer = 0f;


    public Slider healthSlider;

    public float health = 5f;
    public float maxHealth;

    public ParticleSystem bloodEffect;

    public float knockbackTime = .3f;
    private float knockbackTimeCounter;

    public int expDrop = 1;

    public bool isRange = false;

    void Start()
    {
        target = PlayerHealthController.Instance.gameObject;

        maxHealth = health;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            //distance between player & enemy
            distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

            if (knockbackTimeCounter > 0)
            {
                knockbackTimeCounter -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2f;
                }
                
                if (knockbackTimeCounter <= 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed) * .5f;
                }
            }


            //enemy follow player
            if (isRange && distanceToPlayer <= attackRange)
            {
                theRb.velocity = (target.transform.position - transform.position).normalized * moveSpeed * .001f;
            }
            else
            {
                theRb.velocity = (target.transform.position - transform.position).normalized * moveSpeed;
            }

            

            if (theRb.velocity.x > 0)
            {
                spriteRenderer.flipX = true;

            }
            else if (theRb.velocity.x < 0)
            {
                spriteRenderer.flipX = false;
            }



            if (distanceToPlayer <= attackRange)
            {
                anim.SetBool("IsClose", true);
            }
            else
            {
                anim.SetBool("IsClose", false);
            }



            if (hitDelayTimeCounter > 0f)
            {
                hitDelayTimeCounter -= Time.deltaTime;
            }
        }
        else
        {
            anim.SetBool("IsClose", false);
        }
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitDelayTimeCounter <= 0f && health > 0f)
        {
            PlayerHealthController.Instance.TakeDamage(damage);

            hitDelayTimeCounter = hitDelayTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        bloodEffect.Play();

        if (!healthSlider.gameObject.activeSelf)
        {
            healthSlider.gameObject.SetActive(true);
        }

        health -= damageToTake;
        healthSlider.value = health;
       
        if (health <= 0f)
        {
            StartCoroutine(AnimDeath());
        }
        else
        {
            StartCoroutine(AnimDamaged());
        }

        IEnumerator AnimDamaged()
        {
            anim.SetBool("IsAttacked", true);

            DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);

            yield return new WaitForSeconds(.2f);

            anim.SetBool("IsAttacked", false);
        }

        IEnumerator AnimDeath()
        {
            moveSpeed = 0f;

            anim.SetBool("IsDeath", true);

            if (damageToTake + health > 0)
            {
                DamageNumberController.instance.SpawnDamage(damageToTake + health, transform.position);
                ExperienceLevelController.Instance.SpawnExp(transform.position, expDrop);
            }

            yield return new WaitForSeconds(1f);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageToTake, bool isKnockback)
    {
        TakeDamage(damageToTake);
        if (isKnockback)
            {
                knockbackTimeCounter = knockbackTime;
            }
    }
}



