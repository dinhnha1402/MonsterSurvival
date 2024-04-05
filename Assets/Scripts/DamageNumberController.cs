using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumberController : MonoBehaviour
{

    public static DamageNumberController instance;
    public Transform damageCanvas;
    public TMP_Text damageText;

    public float lifeTime;
    private float lifeTimeCounter;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lifeTimeCounter = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTimeCounter > 0)
        {
            lifeTimeCounter -= Time.deltaTime;

            if (lifeTimeCounter <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SpawnDamage(float damageAmount, Vector3 location)
    {
        int rounded = Mathf.RoundToInt(damageAmount);

          
    }
    public void Setup(int damageDisplay)
    {
        lifeTimeCounter = lifeTime;

        damageText.text = damageDisplay.ToString();
    }
}
