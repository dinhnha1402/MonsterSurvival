using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int currentExp;

    public ExpPickup pickup;

    public List<int> expLevels;
    public int currentLevel = 1, levelCount = 50;

    public List<Weapon> weaponsToUpgrade;

    private SaveInfo saveInfo;



    // Start is called before the first frame update
    void Start()
    {
        while(expLevels.Count < levelCount)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }

        string savedData = PlayerPrefs.GetString("SaveGameInfo", "{}");

        saveInfo = JsonUtility.FromJson<SaveInfo>(savedData);

        currentLevel = saveInfo.currentLevel;

        UIController.Instance.UpdateExperience(currentExp, expLevels[currentLevel], currentLevel);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void GetExp(int expAmountToGet)
    {
        currentExp += expAmountToGet;

        //save info
        SaveLoadController.instance.saveInfo.currentExp += expAmountToGet;

        if (currentExp >= expLevels[currentLevel])
        {
            LevelUp();
        }

        UIController.Instance.UpdateExperience(currentExp, expLevels[currentLevel], currentLevel);

    }

    public void SpawnExp(Vector3 position, int expValue)
    {
        Instantiate(pickup, position, Quaternion.identity).expValue = expValue;
    }

    void LevelUp()
    {
        currentExp -= expLevels[currentLevel];

        currentLevel++;

        if (currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        PlayerHealthController.Instance.currentHealth = PlayerHealthController.Instance.maxHealth;
        PlayerHealthController.Instance.healthSlider.maxValue = PlayerHealthController.Instance.maxHealth;
        PlayerHealthController.Instance.healthSlider.value = PlayerHealthController.Instance.currentHealth;

        //save info
        SaveLoadController.instance.saveInfo.currentLevel = currentLevel;

        Time.timeScale = 0f;
        UIController.Instance.levelUpPanel.SetActive(true);
        
        weaponsToUpgrade.Clear();
        List<Weapon> availableWeapons = new List<Weapon>();

        availableWeapons.AddRange(PlayerController.instance.assignedWeapons);



        if(availableWeapons.Count > 0 )
        {
            int selected = Random.Range(0, availableWeapons.Count);

            weaponsToUpgrade.Add(availableWeapons[selected]);

            availableWeapons.RemoveAt(selected);
        }

        if(PlayerController.instance.assignedWeapons.Count + PlayerController.instance.fullyUpgradedWeapons.Count < PlayerController.instance.maxWeapon)
        {
            availableWeapons.AddRange(PlayerController.instance.unassignedWeapons);
        }


        
        for (int i = weaponsToUpgrade.Count; i < PlayerController.instance.maxWeapon; i++)
        {
            if (availableWeapons.Count > 0)
            {
                int selected = Random.Range(0, availableWeapons.Count);

                weaponsToUpgrade.Add(availableWeapons[selected]);

                availableWeapons.RemoveAt(selected);
            }
            
        }




        for (int i = 0; i < weaponsToUpgrade.Count; i++)
        {
            UIController.Instance.upgradeButtons[i].UpdateButtonDisplay(weaponsToUpgrade[i]);
        }

        for(int i = 0; i < UIController.Instance.upgradeButtons.Length; i++)
        {
            if(i < weaponsToUpgrade.Count)
            {
                UIController.Instance.upgradeButtons[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.Instance.upgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

}
