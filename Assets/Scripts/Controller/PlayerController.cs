using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveLoadController;

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
        //LoadWeaponsFromSaveData();
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
        if (weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);

            //save info
            SaveLoadController.instance.saveInfo.assignedWeapons.Add(unassignedWeapons[weaponNumber].gameObject);

            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }
    public void AddWeapon(Weapon weaponToAdd)
    {
        assignedWeapons.Add(weaponToAdd);

        weaponToAdd.gameObject.SetActive(true);

        unassignedWeapons.Remove(weaponToAdd);

        //save info
        SaveLoadController.instance.saveInfo.assignedWeapons.Add(weaponToAdd.gameObject);
    }

    //[System.Serializable]
    //private class WeaponInfo
    //{
    //    public string weaponName;
    //    public int weaponLevel;
    //}

    //[System.Serializable]
    //private class SaveInfoHelper
    //{
    //    public List<WeaponInfo> assignedWeapons; // List to hold weapon names and levels.

    //}

    //public void AddWeapon(WeaponInfo weaponData)
    //{
    //    Weapon weaponToAdd = FindWeaponPrefabByName(weaponData.weaponName);
    //    if (weaponToAdd != null)
    //    {
    //        // Set the weapon level
    //        weaponToAdd.weaponLevel = weaponData.weaponLevel;
    //        // Add to the assigned weapons list
    //        assignedWeapons.Add(weaponToAdd);

    //        weaponToAdd.gameObject.SetActive(true);
    //        // Update the save info
    //        SaveLoadController.instance.saveInfo.assignedWeapons.Add(weaponToAdd.gameObject);
    //        // Remove from unassigned list
    //        unassignedWeapons.Remove(weaponToAdd);
    //    }
    //    else
    //    {
    //        Debug.LogError("Weapon not found for name: " + weaponData.weaponName);
    //    }
    //}

    //private Weapon FindWeaponPrefabByName(string name)
    //{
    //    foreach (Weapon weapon in unassignedWeapons)
    //    {
    //        if (weapon.gameObject.name == name)
    //            return weapon;
    //    }
    //    return null;  // Return null if no matching weapon is found
    //}

    //void LoadWeaponsFromSaveData()
    //{
    //    string savedData = PlayerPrefs.GetString("SaveGameInfo", "{}");
    //    SaveInfoHelper saveInfo = JsonUtility.FromJson<SaveInfoHelper>(savedData);

    //    if (saveInfo != null && saveInfo.assignedWeapons != null)
    //    {
    //        foreach (WeaponInfo weaponData in saveInfo.assignedWeapons)
    //        {
    //            Weapon weaponToAdd = FindWeaponPrefabByName(weaponData.weaponName);
    //            if (weaponToAdd != null)
    //            {
    //                weaponToAdd.weaponLevel = weaponData.weaponLevel;
    //                assignedWeapons.Add(weaponToAdd);
    //                unassignedWeapons.Remove(weaponToAdd);
    //                weaponToAdd.gameObject.SetActive(true);
    //            }
    //            else
    //            {
    //                Debug.LogError("Weapon not found for name: " + weaponData.weaponName);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("No saved weapon data to load.");
    //    }
    //}
}
