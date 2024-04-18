using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
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
