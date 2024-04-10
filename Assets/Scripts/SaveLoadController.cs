using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController instance;

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

    void LoadSavedGame()
    {
        string savedData = PlayerPrefs.GetString("SaveGameInfo", "{}");
        saveInfo = JsonUtility.FromJson<SaveInfo>(savedData);

        // Đối với enemyToSpawn và assignedWeapons, bạn cần phải xử lý chúng riêng.
        // Ví dụ, nếu bạn lưu trữ tên prefab của enemyToSpawn và tên weapon trong PlayerPrefs,
        // bạn sẽ cần phải tìm hoặc load chúng dựa trên tên đó.
        // Điều này thường được thực hiện thông qua Resources.Load, Instantiate, hoặc một hệ thống tương tự.

        // Pseudo-code để tải enemyToSpawn và assignedWeapons
        // saveInfo.enemyToSpawn = LoadEnemiesBasedOnNames(savedEnemyNames);
        // saveInfo.assignedWeapons = LoadWeaponsBasedOnNames(savedWeaponNames);

        // Note: Hãy thực hiện việc tải này dựa trên cách bạn lưu trữ dữ liệu trong PlayerPrefs.
    }
}


[System.Serializable]
public class SaveInfo
{
    public string username;
    public int currentExp;
    public int currentLevel = 1;
    public GameObject[] enemyToSpawn;
    public float waveLength;
    public float minTimeToSpawn;
    public float maxTimeToSpawn;
    public List<Weapon> assignedWeapons;

}
