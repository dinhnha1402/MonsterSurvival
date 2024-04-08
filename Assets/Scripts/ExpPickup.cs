using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : MonoBehaviour
{

    public int expValue;

    private bool movingToPlayer;
    public float moveSpeed;

    public float checkTime;
    private float checkTimeCounter;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.Instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingToPlayer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else 
        {
            checkTimeCounter -= Time.deltaTime;
            if (checkTimeCounter <= 0)
            {
                checkTimeCounter = checkTime;

                if (Vector3.Distance(transform.position, player.transform.position) < player.pickupRange)
                {
                    movingToPlayer = true;
                    moveSpeed += player.moveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceLevelController.Instance.GetExp(expValue);

            Destroy(gameObject);
        }
    }
}
