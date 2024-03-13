using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class MongoDBConnector 
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
        string connectionString = "mongodb+srv://your_username:your_password@your_cluster.mongodb.net";
        client = new MongoDB.Driver.MongoClient(connectionString);
        // Thay thế với tên database thực tế của bạn
        database = client.GetDatabase("your_database_name");
    }

    async void FetchData()
    {
        // Thay thế với tên collection thực tế của bạn
        var collection = database.GetCollection<BsonDocument>("your_collection_name");

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
