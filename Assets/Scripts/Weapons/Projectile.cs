using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float moveSpeed = 3f;

    public float lifeTime = 3f;
    private float lifeTimeCounter;

    private void Start()
    {
        lifeTimeCounter = lifeTime;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;

        transform.localScale -= Vector3.zero * lifeTime * Time.deltaTime;

        lifeTimeCounter -= Time.deltaTime;

        if (lifeTimeCounter <= 0)
        {
            Destroy(gameObject);
        }


    }
}
