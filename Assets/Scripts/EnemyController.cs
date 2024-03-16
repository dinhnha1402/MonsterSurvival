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

    void Start()
    {
        target = PlayerHealthController.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
<<<<<<< Updated upstream
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
=======
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (theRb.velocity.x < 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }



    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.Instance.TakeDamage(damage);
            anim.SetBool("IsClose", true);
>>>>>>> Stashed changes
        }
        else
        {
            anim.SetBool("IsClose", false);
        }
    }
<<<<<<< Updated upstream

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.Instance.TakeDamage(damage);

            hitCounter = hitDelay;
        }

    }




=======
 
>>>>>>> Stashed changes
}



