using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : Weapon
{
    public Transform holder, weaponToSpawn;
    private List<Transform> weaponsToSpawn;

    public float rotateSpeed;

    private Vector3 targetSize;
    private Vector3 targetSizeFix;

    public float lifeTime = 5f;
    private float lifeTimeCounter;

    public float spawnTime = 1.5f;
    private float spawnTimeCounter;


    void Start()
    {


        //fixxxxxxxxxxxxx
        //weaponToSpawn.

        targetSizeFix = holder.localScale;

        SetStats();

        targetSize = targetSizeFix;

        holder.localScale = Vector3.zero;

        lifeTimeCounter = lifeTime;

        spawnTimeCounter = spawnTime;

    }

    // Update is called once per frame
    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));

        holder.localScale = Vector3.MoveTowards(holder.localScale, targetSize, Time.deltaTime);

        lifeTimeCounter -= Time.deltaTime;

        if(lifeTimeCounter <= 0)
        {
            targetSize = Vector3.zero;

            if(holder.localScale == Vector3.zero)
            {
                holder.gameObject.SetActive(false);
            }
        }

        if(!holder.gameObject.activeSelf)
        {
            spawnTimeCounter -= Time.deltaTime;

            if (spawnTimeCounter <= 0)
            {
                spawnTimeCounter = spawnTime;

                holder.gameObject.SetActive(true);

                targetSize = targetSizeFix;

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
        weaponsToSpawn[Mathf.RoundToInt(stats[weaponLevel].amount - 1)].gameObject.SetActive(true);

        for(int i = 0;i < stats[weaponLevel].amount; i++)
        {
            float rotate = (360f / stats[weaponLevel].amount) * i;

            weaponsToSpawn[i].parent.rotation = Quaternion.Euler(0f, 0f, rotate);

            weaponsToSpawn[i].GetComponent<Damager>().damageAmount = stats[weaponLevel].damage;
        }

        targetSizeFix = targetSizeFix * stats[weaponLevel].range;

        rotateSpeed = rotateSpeed * stats[weaponLevel].speed;

        lifeTime = stats[weaponLevel].duration;

        spawnTime = stats[weaponLevel].attackSpeed;

        lifeTimeCounter = 0f;

        spawnTimeCounter = 0f;

    }
}
