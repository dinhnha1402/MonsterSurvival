using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> stats;

    public int weaponLevel;

    [HideInInspector]
    public bool statsUpdated;

    public Sprite icon;

    public void LevelUp()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;

            statsUpdated = true;

            if(weaponLevel >= stats.Count - 1)
            {
                PlayerController.instance.fullyUpgradedWeapons.Add(this);
                PlayerController.instance.assignedWeapons.Remove(this);
            }
        }
    }

}


[System.Serializable]
public class WeaponStats
{
    public float damage, range, speed, duration, attackSpeed, amount;
    public string upgradeText;
}

