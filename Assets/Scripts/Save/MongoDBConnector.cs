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

public class MongoDBConnector : MonoBehaviour
{
    private MongoDB.Driver.MongoClient client;
    private IMongoDatabase database;
    public SaveSystem saveSystem;
    [SerializeField] private TMP_Text loginStatusText;

    [SerializeField] private GameObject loadSaveMenu;
    [SerializeField] private GameObject loginMenu;

    [Serializable]
    public class GameSaveData
    {
        public string username;
        public int score;
        public int level;
        public int waves;
    }


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
            // Tạo filter để kiểm tra tên người dùng
            var filter = Builders<BsonDocument>.Filter.Eq("username", gameSaveData.username);

            // Kiểm tra xem có thông tin người dùng trong DB hay không
            var existingDocument = await collection.Find(filter).FirstOrDefaultAsync();

            if (existingDocument != null)
            {
                // Nếu tên người dùng tồn tại, cập nhật thông tin mới
                var update = Builders<BsonDocument>.Update
                    .Set("currentExp", gameSaveData.currentExp)
                    .Set("currentLevel", gameSaveData.currentLevel)
                    .Set("enemyToSpawn", SerializeGameObjectNames(gameSaveData.enemyToSpawn))
                    .Set("waveLength", gameSaveData.waveLength)
                    .Set("minTimeToSpawn", gameSaveData.minTimeToSpawn)
                    .Set("maxTimeToSpawn", gameSaveData.maxTimeToSpawn)
                    .Set("assignedWeapons", SerializeWeapons(gameSaveData.assignedWeapons));
                await collection.UpdateOneAsync(filter, update);
                Debug.Log("Game info updated successfully.");
            }
            else
            {
                // Nếu tên người dùng không tồn tại, thêm mới thông tin
                var document = new BsonDocument
                {
                    { "username", gameSaveData.username },
                    { "currentExp", gameSaveData.currentExp },
                    { "currentLevel", gameSaveData.currentLevel },
                    { "enemyToSpawn", SerializeGameObjectNames(gameSaveData.enemyToSpawn) },
                    { "waveLength", gameSaveData.waveLength },
                    { "minTimeToSpawn", gameSaveData.minTimeToSpawn },
                    { "maxTimeToSpawn", gameSaveData.maxTimeToSpawn },
                    { "assignedWeapons", SerializeWeapons(gameSaveData.assignedWeapons) }
                };
                await collection.InsertOneAsync(document);
                Debug.Log("Game info added successfully.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
        }
    }

    public BsonDocument SerializeWeaponStat(WeaponStats stat)
    {
        return new BsonDocument
        {
            {"damage", stat.damage},
            {"range", stat.range},
            {"speed", stat.speed},
            {"duration", stat.duration},
            {"attackSpeed", stat.attackSpeed},
            {"amount", stat.amount},
            {"upgradeText", stat.upgradeText}
        };
    }

    public BsonArray SerializeWeapons(List<Weapon> weapons)
    {
        var serializedWeapons = new BsonArray();
        if (weapons == null || weapons.Count == 0)
        {
            // Trả về mảng rỗng nếu không có giá trị
            return serializedWeapons;
        }

        foreach (var weapon in weapons)
        {
            if (weapon != null)
            {
                var stats = new BsonArray();
                if (weapon.stats != null)
                {
                    foreach (var stat in weapon.stats)
                    {
                        stats.Add(SerializeWeaponStat(stat)); // Giả sử SerializeWeaponStat là phương thức đã được định nghĩa
                    }
                }
                else
                {
                    Debug.Log("Weapon stats are null, skipping stats serialization...");
                }

                var weaponDoc = new BsonDocument
            {
                {"weaponLevel", weapon.weaponLevel},
                {"stats", stats}
            };
                serializedWeapons.Add(weaponDoc);
            }
            else
            {
                // Xử lý hoặc bỏ qua Weapon null
                Debug.Log("Encountered a null Weapon, skipping...");
            }
        }
        return serializedWeapons;
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

}


