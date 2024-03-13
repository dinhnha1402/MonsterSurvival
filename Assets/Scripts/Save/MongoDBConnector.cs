using Realms.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;


public class MongoDBConnector 
{
    private MongoDB.Driver.MongoClient client;
    private IMongoDatabase database;

    public MongoDBConnector(string connectionString, string dbName)
    {
        try
        {
            // Tạo MongoClient sử dụng connection string
            client = new MongoDB.Driver.MongoClient(connectionString);

            // Kết nối tới database cụ thể
            database = client.GetDatabase(dbName);

            Console.WriteLine("Kết nối tới MongoDB thành công.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi kết nối tới MongoDB: {ex.Message}");
        }
    }
}
