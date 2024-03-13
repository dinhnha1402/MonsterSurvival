using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class MongoDBConnector : MonoBehaviour
{
    private MongoDB.Driver.MongoClient client;
    private IMongoDatabase database;

    void Start()
    {
        ConnectToMongoDB();
        FetchData();
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
}
