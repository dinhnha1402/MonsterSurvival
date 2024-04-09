using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController Instance;

    public int currentExp;

    public ExpPickup pickup;

    public List<int> expLevels;
    public int currentLevel = 1, levelCount = 50;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        while(expLevels.Count < levelCount)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetExp(int expAmountToGet)
    {
        currentExp += expAmountToGet;

        if(currentExp >= expLevels[currentLevel])
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

        Time.timeScale = 0f;

        UIController.Instance.levelUpPanel.SetActive(true);

        UIController.Instance.upgradeButtons[1].UpdateButtonDisplay(PlayerController.instance.activeWeapon);
       
    }
}
