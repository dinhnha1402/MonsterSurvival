using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SaveFunction : MonoBehaviour
{
    // Start is called before the first frame update
    private IMongoDatabase database;
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
                    .Set("currentWave", gameSaveData.currentWave)
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
                    { "currentWave", gameSaveData.currentWave },
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
}
