using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float spawnTimeCounter;

    public Transform minSpawn, maxSpawn;

    private Transform target;

    private float despawnDistance;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public int checkPerFrame;
    private int enemyToCheck;

    public List<WaveInfo> waves;

    private int currentWave;

    private float waveLengthCounter;

    private SaveInfo saveInfo;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerHealthController.Instance.transform;

        despawnDistance = Vector3.Distance(transform.position, maxSpawn.position) + 4;

        string savedData = PlayerPrefs.GetString("SaveGameInfo", "{}");

        saveInfo = JsonUtility.FromJson<SaveInfo>(savedData);

        currentWave = saveInfo.currentWave - 1;

        GoToNextWave();
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            transform.position = target.position;

            if (currentWave < waves.Count)
            {
                waveLengthCounter -= Time.deltaTime;

                if (waveLengthCounter <= 0)
                {
                    GoToNextWave();
                }

                spawnTimeCounter -= Time.deltaTime;

                if (spawnTimeCounter <= 0)
                {
                    spawnTimeCounter = Random.Range(waves[currentWave].minTimeToSpawn, waves[currentWave].maxTimeToSpawn);

                    GameObject newEnemy = Instantiate(waves[currentWave].enemyToSpawn[Random.Range(0, waves[currentWave].enemyToSpawn.Length)], SelectSpawnPoint(), Quaternion.identity);

                    spawnedEnemies.Add(newEnemy);
                }
            }





            int checkTarget = enemyToCheck + checkPerFrame;

            while (enemyToCheck < checkTarget)
            {
                if (enemyToCheck < spawnedEnemies.Count)
                {
                    if (spawnedEnemies[enemyToCheck] != null)
                    {
                        if (Vector3.Distance(transform.position, spawnedEnemies[enemyToCheck].transform.position) > despawnDistance)
                        {
                            Destroy(spawnedEnemies[enemyToCheck]);

                            spawnedEnemies.RemoveAt(enemyToCheck);

                            checkTarget--;
                        }
                        else
                        {
                            enemyToCheck++;
                        }
                    }
                    else
                    {
                        spawnedEnemies.RemoveAt(enemyToCheck);

                        checkTarget--;
                    }
                }
                else
                {
                    enemyToCheck = 0;

                    checkTarget = 0;
                }
            }

        }
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > 0.5;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            }
            else
            {
                spawnPoint.x = minSpawn.position.x;
            }
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.y = maxSpawn.position.y;
            }
            else
            {
                spawnPoint.y = minSpawn.position.y;
            }
        }

        return spawnPoint;
    }

    public void GoToNextWave()
    {
        currentWave++;

        if (currentWave >= waves.Count)
        {
            currentWave = waves.Count - 1;
        }

        waveLengthCounter = waves[currentWave].waveLength;

        spawnTimeCounter = Random.Range(waves[currentWave].minTimeToSpawn, waves[currentWave].maxTimeToSpawn);

        //save info
        SaveLoadController.instance.saveInfo.enemyToSpawn = waves[currentWave].enemyToSpawn; 
        SaveLoadController.instance.saveInfo.currentWave = currentWave;
        SaveLoadController.instance.saveInfo.waveLength = waves[currentWave].waveLength;
        SaveLoadController.instance.saveInfo.minTimeToSpawn = waves[currentWave].minTimeToSpawn;
        SaveLoadController.instance.saveInfo.maxTimeToSpawn = waves[currentWave].maxTimeToSpawn;
    }
}

[System.Serializable]
public class WaveInfo
{
    public GameObject[] enemyToSpawn;
    public float waveLength = 60f;
    public float minTimeToSpawn = 1f;
    public float maxTimeToSpawn = 3f;

}
