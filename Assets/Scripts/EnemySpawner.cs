using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyToSpawn;
    public float minTimeToSpawn;
    public float maxTimeToSpawn;

    private float spawnTime;
    private float spawnTimeCounter;

    public Transform minSpawn, maxSpawn;

    private Transform target;

    private float despawnDistance;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public int checkPerFrame;
    private int enemyToCheck;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Random.Range(minTimeToSpawn, maxTimeToSpawn);

        spawnTimeCounter = spawnTime;

        target = PlayerHealthController.Instance.transform;

        despawnDistance = Vector3.Distance(transform.position, maxSpawn.position) + 4;

    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {

            spawnTimeCounter -= Time.deltaTime;

            if (spawnTimeCounter <= 0)
            {
                spawnTime = Random.Range(minTimeToSpawn, maxTimeToSpawn);

                spawnTimeCounter = spawnTime;

                GameObject newEnemy = Instantiate(enemyToSpawn[Random.Range(0, enemyToSpawn.Length)], SelectSpawnPoint(), transform.rotation);

                spawnedEnemies.Add(newEnemy);

            }

            transform.position = target.position;



            int checkTarget = enemyToCheck + checkPerFrame;

            while (enemyToCheck < checkTarget)
            {
                if(enemyToCheck < spawnedEnemies.Count)
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
}
