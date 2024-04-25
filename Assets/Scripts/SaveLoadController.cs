using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController instance;
    [SerializeField] private MongoDBConnector mongoController;
    [SerializeField] private GameObject[] allEnemyPrefabs;
    [SerializeField] private GameObject[] allWeaponsPrefabs;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSavedGame();
    }

    public SaveInfo saveInfo;

    [System.Serializable]
    private class SaveInfoHelper
    {
        public string[] enemyToSpawn; // This will hold the names as strings.
        public List<WeaponInfo> assignedWeapons; // List to hold weapon names and levels.

    }

    [System.Serializable]
    public class WeaponInfo
    {
        public string name;
        public int level;
    }

    void LoadSavedGame()
    {
        string savedData = PlayerPrefs.GetString("SaveGameInfo", "{}");

        saveInfo = JsonUtility.FromJson<SaveInfo>(savedData);

        LoadEnemiesAndAssignToSaveInfo(savedData);

        LoadAssignedWeaponsAndSetLevels(savedData);

        //Debug.Log(saveInfo.currentLevel);

        // Đối với enemyToSpawn và assignedWeapons, bạn cần phải xử lý chúng riêng.
        // Ví dụ, nếu bạn lưu trữ tên prefab của enemyToSpawn và tên weapon trong PlayerPrefs,
        // bạn sẽ cần phải tìm hoặc load chúng dựa trên tên đó.
        // Điều này thường được thực hiện thông qua Resources.Load, Instantiate, hoặc một hệ thống tương tự.

        // Pseudo-code để tải enemyToSpawn và assignedWeapons
        // saveInfo.enemyToSpawn = LoadEnemiesBasedOnNames(savedEnemyNames);
        // saveInfo.assignedWeapons = LoadWeaponsBasedOnNames(savedWeaponNames);

        // Note: Hãy thực hiện việc tải này dựa trên cách bạn lưu trữ dữ liệu trong PlayerPrefs.
    }

    public void LoadEnemiesAndAssignToSaveInfo(string savedData)
    {
        // Deserialize to helper to extract enemy names
        SaveInfoHelper helper = JsonUtility.FromJson<SaveInfoHelper>(savedData);

        if (helper != null && helper.enemyToSpawn != null)
        {
            // Prepare the array to hold matched GameObjects
            GameObject[] matchedEnemies = new GameObject[helper.enemyToSpawn.Length];

            for (int i = 0; i < helper.enemyToSpawn.Length; i++)
            {
                // Match each enemy name to a prefab
                matchedEnemies[i] = FindEnemyPrefabByName(helper.enemyToSpawn[i]);
            }

            // Assuming saveInfo is already created and part of this component
            saveInfo.enemyToSpawn = matchedEnemies; // Assign matched GameObjects to saveInfo
        }
        else
        {
            Debug.LogError("Failed to load or parse enemy data.");
        }
    }

    private GameObject FindEnemyPrefabByName(string name)
    {
        // Example using a predefined list or dictionary
        foreach (GameObject prefab in allEnemyPrefabs) // Assume allEnemyPrefabs is populated elsewhere
        {
            if (prefab.name == name)
                return prefab;
        }
        Debug.LogError("Prefab not found for name: " + name);
        return null;
    }

    public void LoadAssignedWeaponsAndSetLevels(string savedData)
    {
        // Deserialize to helper to extract weapon names and levels
        SaveInfoHelper helper = JsonUtility.FromJson<SaveInfoHelper>(savedData);

        if (helper != null && helper.assignedWeapons != null)
        {
            List<GameObject> matchedWeapons = new List<GameObject>();

            foreach (var weaponInfo in helper.assignedWeapons)
            {
                GameObject weaponPrefab = FindWeaponPrefabByName(weaponInfo.name);
                if (weaponPrefab != null)
                {
                    GameObject weaponInstance = Instantiate(weaponPrefab); // Assuming instantiation is needed
                    Weapon weaponComponent = weaponInstance.GetComponent<Weapon>();
                    if (weaponComponent != null)
                    {
                        weaponComponent.weaponLevel = weaponInfo.level;
                    }
                    matchedWeapons.Add(weaponInstance);
                }
            }

            // Assign matched GameObjects to saveInfo
            saveInfo.assignedWeapons = matchedWeapons;
        }
        else
        {
            Debug.LogError("Failed to load or parse weapon data.");
        }
    }
    private GameObject FindWeaponPrefabByName(string name)
    {
        foreach (GameObject prefab in allWeaponsPrefabs)
        {
            if (prefab.name == name)
                return prefab;
        }
        Debug.LogError("Weapon prefab not found for name: " + name);
        return null;
    }

    public void SaveToDB()
    {
        SaveSystem saveSystem = new SaveSystem();
        string username = saveSystem.GetUsername();

        // Tạo một đối tượng SaveInfo mới
        SaveInfo data = new SaveInfo
        {
            username = username,
            currentExp = instance.saveInfo.currentExp,
            currentLevel = instance.saveInfo.currentLevel,
            currentWave = instance.saveInfo.currentWave,
            waveLength = instance.saveInfo.waveLength,
            minTimeToSpawn = instance.saveInfo.minTimeToSpawn,
            maxTimeToSpawn = instance.saveInfo.maxTimeToSpawn,
            enemyToSpawn = instance.saveInfo.enemyToSpawn,
            assignedWeapons = instance.saveInfo.assignedWeapons,
            saveDateTime = DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy")
        };
        // Serialize to JSON
        string jsonGameSaveData = JsonUtility.ToJson(data, true);  // Sử dụng `true` để định dạng JSON cho dễ đọc

        saveSystem.SaveGame(jsonGameSaveData);

        // Giả sử mongoController đã được khởi tạo và có thể sử dụng
        mongoController.SaveGameInfo(data);
    }
}



[System.Serializable]
public class SaveInfo
{
    public string username;
    public int currentExp;
    public int currentLevel = 1;
    public GameObject[] enemyToSpawn;
    public int currentWave;
    public float waveLength;
    public float minTimeToSpawn;
    public float maxTimeToSpawn;
    public List<GameObject> assignedWeapons;
    public string saveDateTime;
   
}
