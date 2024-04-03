using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : MonoBehaviour
{
    public float rotateSpeed;

    public Transform holder, weaponToSpawn;

    public float timeBetweenSpawn;

    private float spawnCounter;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));

        spawnCounter -= Time.deltaTime;
        if(spawnCounter <= 0)
        {
            spawnCounter = timeBetweenSpawn;

            Instantiate(weaponToSpawn, weaponToSpawn.position, weaponToSpawn.rotation, holder).gameObject.SetActive(true);
        }

    }
}
