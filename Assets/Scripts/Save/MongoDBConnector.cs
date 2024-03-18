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
    private SaveSystem saveSystem;


    [SerializeField] private TMP_Text loginStatusText;

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
                saveSystem.SaveUsername(username);
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
    }

    public async void RegisterUser(string username, string password)
    {
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
        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing MongoDB: {e.Message}");
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
}
