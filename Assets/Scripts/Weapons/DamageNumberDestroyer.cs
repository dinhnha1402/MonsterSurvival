using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberDestroyer : MonoBehaviour
{
    public float lifeTime;
    private float lifeTimeCounter;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimeCounter = lifeTime;

        //Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * lifeTime * Time.deltaTime;

        if (lifeTimeCounter > 0)
        {
            lifeTimeCounter -= Time.deltaTime;
            if (lifeTimeCounter <= 0)
            {
                DamageNumberController.instance.PlaceInPool(this.gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
