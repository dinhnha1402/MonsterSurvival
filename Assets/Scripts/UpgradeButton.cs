using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{

    public TMP_Text nameLevelText, upgradeDescText;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    public void UpdateButtonDisplay(Weapon theWeapon)
    {
        if (theWeapon.gameObject.activeSelf)
        {
            upgradeDescText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;

            weaponIcon.sprite = theWeapon.icon;

            nameLevelText.text = theWeapon.name + " Level " + theWeapon.weaponLevel;
        } 
        else
        {
            upgradeDescText.text = "Unlock" + theWeapon.name;

            weaponIcon.sprite = theWeapon.icon;

            nameLevelText.text = theWeapon.name;
        }

        assignedWeapon = theWeapon;
    }

    public void SelectUpgrade()
    {
        if (assignedWeapon != null)
        {
            if (assignedWeapon.gameObject.activeSelf)
            {
                assignedWeapon.LevelUp();
            }
            else
            {
                PlayerController.instance.AddWeapon(assignedWeapon);
            }


            UIController.Instance.levelUpPanel.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}
