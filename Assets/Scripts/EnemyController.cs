using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRb;
    public float moveSpeed;
    private Transform target;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public float damage;
    public float attackRange = 0f;
    //private float distanceToPlayer = 0f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //enemy follow player
        theRb.velocity = (target.position - transform.position).normalized * moveSpeed;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

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


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthController.Instance.TakeDamage(damage);
            Debug.Log(damage);
        }
        
    }


}



