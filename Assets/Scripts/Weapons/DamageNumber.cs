using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float lifeTime;
    private float lifeTimeCounter;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (lifeTimeCounter > 0)
        {
            lifeTimeCounter -= Time.deltaTime;

            if (lifeTimeCounter <= 0)
            {
                DamageNumberController.instance.PlaceInPool(this.gameObject);
            }
        }

        transform.position += Vector3.up * lifeTime * Time.deltaTime;
    }

    public void Setup(int damageDisplay)
    {
        lifeTimeCounter = lifeTime;

        this.GetComponent<TMP_Text>().text = damageDisplay.ToString();
    }
}
