using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController instance;

    private void Awake()
    {
        instance = this;
    }

    public SaveInfo saveInfo;
}
[System.Serializable]
public class SaveInfo
{
    public int currentExp;
    public int currentLevel = 1;
    public GameObject[] enemyToSpawn;
    public float waveLength;
    public float minTimeToSpawn;
    public float maxTimeToSpawn;
    public List<Weapon> assignedWeapons;

}
