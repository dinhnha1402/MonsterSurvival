using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public Damager damager;
    public Projectile projectile;
    public Transform shotWeapon;

    public float shotTime = 1f;
    private float shotTimeCounter;

    public float weaponRange;

    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        SetStats();

        shotTimeCounter = shotTime;
    }

    // Update is called once per frame
    void Update()
    {
        shotTimeCounter -= Time.deltaTime;

        if (shotTimeCounter <= 0)
        {
            shotTimeCounter = shotTime;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange, whatIsEnemy);

            if(enemies.Length > 0)
            {
                for (int i = 0; i < stats[weaponLevel].amount; i++)
                    
                {
                    //calculate facing direction between enemies and player
                    Vector3 targetPosition = enemies[Random.Range(0, enemies.Length)].transform.position;

                    Vector3 direction = targetPosition - transform.position;

                    float angle = Mathf.Atan2(direction.y, direction.x) *Mathf.Rad2Deg;

                    angle -= 90;

                    //set projectile angle
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    
                    shotWeapon.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);

                    StartCoroutine(AnimShot());

                    
                }
            }
        }


        if (statsUpdated)
        {
            statsUpdated = false;

            SetStats();
        }
    }

    void SetStats()
    {
        damager.damageAmount = stats[weaponLevel].damage;

        weaponRange = stats[weaponLevel].range;

        projectile.moveSpeed = stats[weaponLevel].speed;

        projectile.lifeTime = stats[weaponLevel].duration;

        shotTime = stats[weaponLevel].attackSpeed;

        shotTimeCounter = 0f;

    }

    IEnumerator AnimShot()
    {
        shotWeapon.GetComponent<Animator>().SetBool("IsShot", true);

        yield return new WaitForSeconds(.1f);

        shotWeapon.GetComponent<Animator>().SetBool("IsShot", false);
    }


}
