using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : MonoBehaviour
{
    public Transform holder, weaponToSpawn;

    public float rotateSpeed;

    private Vector3 targetSize;
    private Vector3 targetSizeFix;

    public float lifeTime = 5f;
    private float lifeTimeCounter;

    public float spawnTime = 1.5f;
    private float spawnTimeCounter;


    void Start()
    {
        targetSizeFix = weaponToSpawn.localScale;

        targetSize = targetSizeFix;

        weaponToSpawn.localScale = Vector3.zero;

        lifeTimeCounter = lifeTime;

        spawnTimeCounter = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));

        weaponToSpawn.localScale = Vector3.MoveTowards(weaponToSpawn.localScale, targetSize, 8f * Time.deltaTime);

        lifeTimeCounter -= Time.deltaTime;

        if(lifeTimeCounter <= 0)
        {
            targetSize = Vector3.zero;

            if(weaponToSpawn.localScale == Vector3.zero)
            {
                weaponToSpawn.gameObject.SetActive(false);
            }
        }

        if(!weaponToSpawn.gameObject.activeSelf)
        {
            spawnTimeCounter -= Time.deltaTime;

            if (spawnTimeCounter <= 0)
            {
                weaponToSpawn.gameObject.SetActive(true);

                targetSize = targetSizeFix;

                lifeTimeCounter = lifeTime;

                spawnTimeCounter = spawnTime;
            }
        }
    }
}