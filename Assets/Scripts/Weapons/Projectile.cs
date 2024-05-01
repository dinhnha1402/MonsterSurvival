using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float moveSpeed = 3f;

    public float lifeTime = 3f;
    private float lifeTimeCounter;

    private Vector3 targetSize;

    void Start()
    {
        lifeTimeCounter = lifeTime;

        targetSize = transform.localScale * 2;

    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, moveSpeed * Time.deltaTime);
      
        transform.position += transform.up * moveSpeed * Time.deltaTime;

        lifeTimeCounter -= Time.deltaTime;

        if (lifeTimeCounter <= 0)
        {
            Destroy(gameObject);
        }


    }
}
