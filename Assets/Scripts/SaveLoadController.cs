using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController instance;
    [SerializeField] private MongoDBConnector mongoController;

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
            waveLength = instance.saveInfo.waveLength,
            minTimeToSpawn = instance.saveInfo.minTimeToSpawn,
            maxTimeToSpawn = instance.saveInfo.maxTimeToSpawn,
            enemyToSpawn = instance.saveInfo.enemyToSpawn,
            assignedWeapons = instance.saveInfo.assignedWeapons,
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
    public float waveLength;
    public float minTimeToSpawn;
    public float maxTimeToSpawn;
    public List<Weapon> assignedWeapons;

}
