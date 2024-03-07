using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRb;
    public float moveSpeed;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        theRb.velocity = (target.position - transform.position).normalized * moveSpeed;
    }
}
