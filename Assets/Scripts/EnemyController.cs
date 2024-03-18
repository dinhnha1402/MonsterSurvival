using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRb;

    private GameObject target;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed;
    public float damage;
    public float attackRange = 0f;
    public float hitDelay = 1f;
    private float hitCounter;
    private float distanceToPlayer = 0f;

    public float health = 5f;

    void Start()
    {
        target = PlayerHealthController.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            //enemy follow player
            theRb.velocity = (target.transform.position - transform.position).normalized * moveSpeed;

            //distance between player & enemy
            distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);


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


            if (hitCounter > 0f)
            {
                hitCounter -= Time.deltaTime;
            }
        }
        else
        {
            anim.SetBool("IsClose", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.Instance.TakeDamage(damage);

            hitCounter = hitDelay;
        }

    }

    //public void TakeDamage(float damageToTake)
    //{
    //    health -= damageToTake;

    //    anim.SetBool("IsAttacked", true);

    //    Invoke("ResetIsAttacked", 0.1f);

    //    if (health <= 0f)
    //    {
    //        Destroy(gameObject, 1f);
    //    }
    //}

    //private void ResetIsAttacked()
    //{
    //    anim.SetBool("IsAttacked", false);
    //}

    private float lastHitTime;

    public void TakeDamage(float damageToTake)
    {
        anim.SetBool("IsAttacked", true);

        if (Time.time - lastHitTime < 0.5f)
        {

            anim.SetBool("IsAttacked", true);
        }
        else
        {

            anim.SetBool("IsAttacked", false);
        }


        lastHitTime = Time.time;


        health -= damageToTake;

     
        if (health <= 0f)
        {
            Destroy(gameObject, 1f);
        }
    }
}



