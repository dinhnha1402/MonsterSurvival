using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime.Documents;
using TMPro;
using Assets.Scripts.Save;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

public class MongoDBConnector : MonoBehaviour
{
    private MongoDB.Driver.MongoClient client;
    private IMongoDatabase database;
    public SaveSystem saveSystem;
    [SerializeField] private TMP_Text loginStatusText;

    [SerializeField] private GameObject loadSaveMenu;
    [SerializeField] private GameObject loginMenu;

    [SerializeField] private GameObject userPrefab;  // Prefab for user entries
    [SerializeField] private Transform container;


    void Start()
    {
        ConnectToMongoDB();
        //FetchData();
    }

    void ConnectToMongoDB()
    {
        // Thay thế với connection string thực tế của bạn
        string connectionString = "mongodb+srv://dbadmin:Pcc-0903907623@cluster01.dx0ubl0.mongodb.net/?retryWrites=true&w=majority&appName=Cluster01";
        client = new MongoDB.Driver.MongoClient(connectionString);
        // Thay thế với tên database thực tế của bạn
        database = client.GetDatabase("MonsterSurvivalDB");
    }

    async void FetchData()
    {
        // Thay thế với tên collection thực tế của bạn
        var collection = database.GetCollection<BsonDocument>("UserAccount");

        try
        {
            var documents = await collection.Find(new BsonDocument()).ToListAsync();
            foreach (var document in documents)
            {
                Debug.Log(document.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
        }
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public async void CheckLogin(string username, string password)
    {
        var hashedPassword = HashPassword(password);

        var collection = database.GetCollection<BsonDocument>("UserAccount");


        try
        {
            // Specify the query to find the user with the given username and password
            var filter = new BsonDocument
            {
                { "username", username },
                { "password", hashedPassword }
            };

            var document = await collection.Find(filter).FirstOrDefaultAsync();

            if (document != null)
            {

                loginStatusText.text = "Login successful!";
                OpenLoadSaveGameMenu();
                saveSystem.SaveUsername(username);
                CreatePrefabsForUser(username);

                string user = saveSystem.GetUsername();

            }
            else
            {
                loginStatusText.text = "Invalid username or password";
            }

        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
        }

        if (!loginStatusText.gameObject.activeInHierarchy)
        {
            loginStatusText.gameObject.SetActive(true);
        }

    }

    public async void RegisterUser(string username, string password)
    {

        if (username == "" || password == "")
        {
            if (!loginStatusText.gameObject.activeInHierarchy)
            {
                loginStatusText.gameObject.SetActive(true);
            }

            loginStatusText.text = "Username or password can't be empty";
            return;
        }

        var collection = database.GetCollection<UserAccount>("UserAccount");

        try
        {
            // Kiểm tra xem username đã tồn tại chưa
            var existingUserFilter = new BsonDocument { { "username", username } };
            var existingUser = await collection.Find(existingUserFilter).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                // Nếu username đã tồn tại
                loginStatusText.text = "Username already exists";
                return;
            }

            // Nếu username chưa tồn tại, tạo tài khoản mới

            var hashPassword = HashPassword(password);

            // Nếu username chưa tồn tại, tạo tài khoản mới
            var newUser = new UserAccount
            {
                Username = username,
                Password = hashPassword
            };

            await collection.InsertOneAsync(newUser);
            loginStatusText.text = "Registration successful!";
            OpenLoadSaveGameMenu();

        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
        }

        if (!loginStatusText.gameObject.activeInHierarchy)
        {
            loginStatusText.gameObject.SetActive(true);
        }

    }

    public async void AddOrUpdateUserScore(string username, int score)
    {

        var collection = database.GetCollection<UserScore>("UserScore");

        try
        {
            var existingUserFilter = new BsonDocument { { "username", username } };
            var existingUser = await collection.Find(existingUserFilter).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                // Người dùng tồn tại, cập nhật điểm số của họ
                var updateFilter = Builders<UserScore>.Filter.Eq("Username", username);
                var updateDefinition = Builders<UserScore>.Update.Set("Score", score);
                await collection.UpdateOneAsync(updateFilter, updateDefinition);
            }
            else
            {
                var newUserScore = new UserScore
                {
                    Username = username,
                    Score = score
                };

                await collection.InsertOneAsync(newUserScore);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
        }
    }

    public void OpenLoadSaveGameMenu()
    {
        loadSaveMenu.SetActive(true);
        loginMenu.SetActive(false);
    }

    public async void SaveGameInfo(SaveInfo gameSaveData)
    {
        var collection = database.GetCollection<BsonDocument>("UserSaveInfo");

        try
        {
            // Tạo filter để kiểm tra thời gian lưu trùng
            var filter = Builders<BsonDocument>.Filter.Eq("saveDateTime", gameSaveData.saveDateTime);

            // Kiểm tra xem đã có thông tin lưu tại thời điểm này chưa
            var existingDocument = await collection.Find(filter).FirstOrDefaultAsync();

            if (existingDocument == null)
            {
                // Nếu không có dữ liệu trùng thời gian lưu, thêm mới thông tin
                var document = new BsonDocument
            {
                { "username", gameSaveData.username },
                { "currentExp", gameSaveData.currentExp },
                { "currentLevel", gameSaveData.currentLevel },
                { "enemyToSpawn", SerializeGameObjectNames(gameSaveData.enemyToSpawn) },
                { "currentWave", gameSaveData.currentWave },
                { "waveLength", gameSaveData.waveLength },
                { "minTimeToSpawn", gameSaveData.minTimeToSpawn },
                { "maxTimeToSpawn", gameSaveData.maxTimeToSpawn },
                { "assignedWeapons", SerializeWeapons(gameSaveData.assignedWeapons) },
                { "saveDateTime", gameSaveData.saveDateTime }
            };
                await collection.InsertOneAsync(document);
                Debug.Log("Game info added successfully.");
            }
            else
            {
                Debug.Log("Duplicate save time detected; no data added.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
        }
    }

    public BsonArray SerializeWeapons(List<GameObject> gameObjects)
    {
        var namesAndLevels = new BsonArray();
        if (gameObjects == null || gameObjects.Count == 0)
        {
            // Return an empty array if there are no GameObjects
            return namesAndLevels;
        }

        foreach (var gameObject in gameObjects)
        {
            if (gameObject != null)
            {
                // Get the Weapon component and retrieve the weaponLevel
                var weapon = gameObject.GetComponent<Weapon>();
                if (weapon != null)
                {
                    //string cleanedName = Regex.Replace(gameObject.name, @" \(\d+\)| \d+", "");
                    var nameAndLevel = new BsonDocument
                {
                    { "name", gameObject.name },
                    { "weaponLevel", weapon.weaponLevel }
                };
                    namesAndLevels.Add(nameAndLevel);
                }
                else
                {
                    // Log if the Weapon component is missing
                    Debug.Log("GameObject '" + gameObject.name + "' does not have a Weapon component, skipping...");
                }
            }
            else
            {
                // Handle or skip null GameObject
                Debug.Log("Encountered a null GameObject, skipping...");
            }
        }
        return namesAndLevels;
    }


    public BsonArray SerializeGameObjectNames(GameObject[] gameObjects)
    {
        var names = new BsonArray();
        if (gameObjects == null || gameObjects.Length == 0)
        {
            // Trả về mảng rỗng nếu không có giá trị
            return names;
        }

        foreach (var gameObject in gameObjects)
        {
            if (gameObject != null)
            {
                names.Add(gameObject.name);
            }
            else
            {
                // Xử lý hoặc bỏ qua GameObject null
                Debug.Log("Encountered a null GameObject, skipping...");
            }
        }
        return names;
    }

    async void CreatePrefabsForUser(string username)
    {
        var collection = database.GetCollection<BsonDocument>("UserSaveInfo");

        var filter = Builders<BsonDocument>.Filter.Eq("username", username);

        try
        {
            var documents = await collection.Find(filter).ToListAsync();

            foreach (var document in documents)
            {
                GameObject newUserPrefab = Instantiate(userPrefab, container);
                newUserPrefab.name = document["username"].AsString;

                // Accessing TextMeshPro components by child GameObject names
                var usernameText = newUserPrefab.transform.Find("UsernameText").GetComponent<TextMeshProUGUI>();
                var dateTimeText = newUserPrefab.transform.Find("DateTimeText").GetComponent<TextMeshProUGUI>();

                // Setting text values
                usernameText.text = document["username"].AsString;
                dateTimeText.text = document["saveDateTime"].AsString;

                // Set the SaveInfo data on the prefab
                var saveInfoComponent = newUserPrefab.GetComponent<SaveInfoComponent>();
                var prefabController = newUserPrefab.GetComponent<SaveInfoComponent>();
                document.Remove("_id");
                saveInfoComponent.saveInfo = document.ToJson();
                /*prefabController.Setup()*/;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
        }
    }

    private List<Weapon> LoadWeapons(BsonArray weaponData)
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (var item in weaponData)
        {
            Weapon weapon = new Weapon();
            weapon.weaponLevel = item["weaponLevel"].AsInt32;
            weapon.stats = new List<WeaponStats>();

            foreach (var stat in item["stats"].AsBsonArray)
            {
                weapon.stats.Add(new WeaponStats
                {
                    damage = (float)stat["damage"].ToDouble(),
                    range = (float)stat["range"].ToDouble(),
                    speed = (float)stat["speed"].ToDouble(),
                    duration = (float)stat["duration"].ToDouble(),
                    attackSpeed = (float)stat["attackSpeed"].ToDouble(),
                    amount = (int)stat["amount"].ToDouble(),
                    upgradeText = stat["upgradeText"].AsString
                });
            }

            weapons.Add(weapon);
        }
        return weapons;
    }

    //private GameObject[] LoadEnemyGameObjects(BsonArray enemyIds)
    //{
    //    GameObject[] enemies = new GameObject[enemyIds.Count];
    //    for (int i = 0; i < enemyIds.Count; i++)
    //    {
    //        string enemyName = enemyIds[i].AsString;
    //        enemies[i] = FindEnemyPrefabByName(enemyName);
    //    }
    //    return enemies;
    //}

    // Example utility method to find enemy prefab by name
    //private GameObject FindEnemyPrefabByName(string name)
    //{
    //    // Assuming all enemy prefabs are loaded and stored in a dictionary for quick lookup
    //    if (enemyPrefabDictionary.TryGetValue(name, out GameObject prefab))
    //    {
    //        return prefab;
    //    }
    //    else
    //    {
    //        Debug.LogError("Prefab not found for enemy: " + name);
    //        return null;
    //    }
    //}

}


