using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    public float moveSpeed;
    public Animator anim;
    public SpriteRenderer spriteRenderer;


    public Vector2 moveDir;

    public float pickupRange = 1.5f;

    public List<Weapon> unassignedWeapons, assignedWeapons;

    public int maxWeapon = 3;

    [HideInInspector]
    public List<Weapon> fullyUpgradedWeapons = new List<Weapon>();

    // Start is called before the first frame update
    void Start()
    {
        AddWeapon(Random.Range(0, unassignedWeapons.Count));
    }
    // Update is called once per frame

    void Update()
    {
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");


        moveInput.Normalize();

        // Update moveDir based on moveInput
        moveDir = new Vector2(moveInput.x, moveInput.y);

        if (anim.GetBool("IsDeath") == false)
        {
            transform.position += moveInput * moveSpeed * Time.deltaTime;




            if (moveInput != Vector3.zero)
            {
                anim.SetBool("IsMoving", true);

                if (moveInput.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                if (moveInput.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }

        }
    }

    public void AddWeapon(int weaponNumber)
    {
        if(weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);

            unassignedWeapons.RemoveAt(weaponNumber);
        }
        //save info
        SaveLoadController.instance.saveInfo.assignedWeapons.Add(unassignedWeapons[weaponNumber]);

    }
    public void AddWeapon(Weapon weaponToAdd)
    {
        assignedWeapons.Add(weaponToAdd);

        weaponToAdd.gameObject.SetActive(true);

        unassignedWeapons.Remove(weaponToAdd);

        //save info
        SaveLoadController.instance.saveInfo.assignedWeapons.Add(weaponToAdd);
    }
}
