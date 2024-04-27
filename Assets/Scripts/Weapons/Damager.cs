using MongoDB.Bson.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{

    public float damageAmount;

    public bool isKnockback;

    public bool isDestroyOnImpact;

    public bool isDamageOvertime;

    public float hitDelayTime = 1f;
    private float hitDelayTimeCounter;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDamageOvertime)
        {
            hitDelayTimeCounter -= Time.deltaTime;
        
            if(hitDelayTimeCounter <= 0f )
            {
                hitDelayTimeCounter = hitDelayTime;

                for( int i = 0; i < enemiesInRange.Count; i++ )
                {
                    if(enemiesInRange[i] != null)
                    {
                        enemiesInRange[i].TakeDamage(damageAmount, isKnockback);
                    }
                    else
                    {
                        enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
        
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !isDamageOvertime)
        {
            collision.GetComponent<EnemyController>().TakeDamage(damageAmount, isKnockback);

            if(isDestroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.tag == "Enemy" && isDamageOvertime)
        {
            enemiesInRange.Add(collision.GetComponent<EnemyController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && isDamageOvertime)
        {
            enemiesInRange.Remove(collision.GetComponent<EnemyController>());
        }
    }
}
