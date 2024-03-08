using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRb;
    public float moveSpeed;
    private Transform target;
    public Animator anim;

    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //enemy follow player
        theRb.velocity = (target.position - transform.position).normalized * moveSpeed;

        if (theRb.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (theRb.velocity.x < 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            {
                PlayerHealthController.Instance.TakeDamage(damage);
                anim.SetBool("IsClose", true);
            }
        else if (collision.gameObject.tag != "Player")
        {
            anim.SetBool("IsClose", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("IsClose", false);

    }


}
