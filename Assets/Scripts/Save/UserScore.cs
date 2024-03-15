using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserScore
{
    [BsonId] // Đánh dấu trường này là ID duy nhất của document.
    public ObjectId _id { get; set; }

    [BsonElement("username")] // Tùy chọn này chỉ rõ tên trường khi được lưu trong MongoDB.
    public string Username { get; set; }

    [BsonElement("score")]
    public int Score { get; set; }
}
