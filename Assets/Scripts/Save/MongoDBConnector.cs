using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime.Documents;
using TMPro;

public class MongoDBConnector : MonoBehaviour
{
    private MongoDB.Driver.MongoClient client;
    private IMongoDatabase database;

    public TMP_Text loginStatusText;

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
        var filter = new BsonDocument {
            { "username", username },
            { "password", password }
        };

        var collection = database.GetCollection<BsonDocument>("UserAccount");

        try
        {
            var documents = await collection.Find(filter).ToListAsync();
            if (documents.Count > 0)
            {
                loginStatusText.text = "Login thành công";
            }
            else
            {
                loginStatusText.text = "Login thất bại";
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during login check: {e.Message}");
        }
    }
}
