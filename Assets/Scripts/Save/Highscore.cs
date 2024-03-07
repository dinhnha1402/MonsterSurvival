using MongoDB.Bson;
using Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : RealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId? Id { get; set; }

    [MapTo("_partition")]
    public string Partition { get; set; }

    [MapTo("player")]
    public string Player { get; set; }

    [MapTo("score")]
    public int? Score { get; set; }

    public Highscore()
    {
        Id = ObjectId.GenerateNewId();
        Partition = "MonsterSurvival";
    }
}
