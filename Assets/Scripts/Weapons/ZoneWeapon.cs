using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class ZoneWeapon : Weapon
{
    public Transform weaponToSpawn, weaponEffect;

    public float lifeTime = 5f;
    private float lifeTimeCounter;

    public float spawnTime = 1.5f;
    private float spawnTimeCounter;

    private Vector3 targetSize;
    private Vector3 targetSizeFix;

    private Vector3 effectSize;
    private Vector3 effectSizeFix;


    void Start()
    {
        targetSizeFix = weaponToSpawn.localScale;
        effectSizeFix = weaponEffect.localScale;

        SetStats();

        targetSize = targetSizeFix;
        effectSize = effectSizeFix;

        weaponToSpawn.localScale = Vector3.zero;
        weaponEffect.localScale = Vector3.zero;

        lifeTimeCounter = lifeTime;

        spawnTimeCounter = spawnTime;
    }

    private void Update()
    {
        weaponToSpawn.localScale = Vector3.MoveTowards(weaponToSpawn.localScale, targetSize, Time.deltaTime * 2f);
        weaponEffect.localScale = Vector3.MoveTowards(weaponEffect.localScale, effectSize, Time.deltaTime * 10f);

        lifeTimeCounter -= Time.deltaTime;

        if (lifeTimeCounter <= 0)
        {
            targetSize = Vector3.zero;
            effectSize = Vector3.zero;

            if (weaponToSpawn.localScale == Vector3.zero)
            {
                weaponToSpawn.gameObject.SetActive(false);
            }
        }

        if (!weaponToSpawn.gameObject.activeSelf)
        {
            spawnTimeCounter -= Time.deltaTime;

            if (spawnTimeCounter <= 0)
            {
                spawnTimeCounter = spawnTime;

                weaponToSpawn.gameObject.SetActive(true);

                targetSize = targetSizeFix;
                effectSize = effectSizeFix;


                lifeTimeCounter = lifeTime;

            }
        }

        if (statsUpdated)
        {
            statsUpdated = false;

            SetStats();
        }
    }

    public void SetStats()
    {
        weaponToSpawn.GetComponent<Damager>().damageAmount = stats[weaponLevel].damage;

        targetSizeFix = targetSizeFix * stats[weaponLevel].range;
        effectSizeFix = effectSizeFix * stats[weaponLevel].range;

        weaponToSpawn.GetComponent<Damager>().hitDelayTime = stats[weaponLevel].speed;

        lifeTime = stats[weaponLevel].duration;

        spawnTime = stats[weaponLevel].attackSpeed;

        lifeTimeCounter = 0f;

        spawnTimeCounter = 0f;

    }
}
    

