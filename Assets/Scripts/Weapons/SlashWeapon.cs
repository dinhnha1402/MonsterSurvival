using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class SlashWeapon : Weapon
{

    public Damager damager;
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       damager.transform.rotation = Quaternion.Euler(0f, 0f, damager.transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
    }
}
