using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{

    public float damageAmount;

    public float lifeTime;
    public float growSpeed = 4f;

    private Vector3 targetSize;


    // Start is called before the first frame update
    void Start()
    {
        

        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            targetSize = Vector3.zero;

            if(transform.localScale.x == 0f)
            {
                Destroy(transform.parent.gameObject, lifeTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().TakeDamage(damageAmount);
        }
    }
}
