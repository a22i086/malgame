using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public List<GameObject> animalPrefabs; // 召喚する動物のプレハブ
    public Transform spawnPoint; // 動物を召喚する位置
    public float spawnInterval = 5f; // 召喚の間隔
    public float waitTimeAfterSpawn = 2f;
    private float spawnTimer = 0f;
    public GameManager gameManager; // ゲームマネージャーへの参照
    public EnemyAlert enemyAlert; // アラートアイコンの表示

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemyAlert = FindObjectOfType<EnemyAlert>();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnAnimal();
            spawnTimer = 0f;
        }
    }

    void SpawnAnimal()
    {
        GameObject selectedAnimalPrefab = animalPrefabs[Random.Range(0, animalPrefabs.Count)];
        Vector3 spawnPosition = spawnPoint.position;
        spawnPosition.z -= 3.0f;
        spawnPosition.y += 3.0f;
        GameObject animalInstance = Instantiate(selectedAnimalPrefab, spawnPoint.position, Quaternion.identity);
        Character animalCharacter = animalInstance.GetComponent<Character>();
        animalCharacter.team = 1;
        animalCharacter.isPlayerControlled = false;
        animalCharacter.isSpawnConfirmed = true;
        gameManager.AddBotAnimal(animalCharacter);

        if (enemyAlert != null)
        {
            enemyAlert.AddEnemy(animalInstance.transform);
        }

        StartCoroutine(MoveAnimalAfterWait(animalCharacter));
    }

    // IEnumerator MoveAnimalAfterWait(Character animalCharacter)
    // {
    //     yield return new WaitForSeconds(waitTimeAfterSpawn);

    //     while (animalCharacter != null && animalCharacter.gameObject != null && animalCharacter.gameObject.activeInHierarchy)
    //     {
    //         List<Character> enemies = gameManager.GetEnemies(animalCharacter);
    //         bool enemyInRange = false;
    //         Character nearestEnemy = null;
    //         float shortestDistance = Mathf.Infinity;

    //         foreach (Character enemy in enemies)
    //         {
    //             float distanceToEnemy = Vector3.Distance(animalCharacter.transform.position,
    //                                                         enemy.transform.position);
    //             if (distanceToEnemy < shortestDistance)
    //             {
    //                 shortestDistance = distanceToEnemy;
    //                 nearestEnemy = enemy;
    //             }
    //             if (distanceToEnemy <= animalCharacter.attackRange)
    //             {
    //                 enemyInRange = true;
    //                 break;
    //             }
    //         }

    //         if (!enemyInRange)
    //         {
    //             Vector3 position = animalCharacter.transform.position;
    //             position.z -= animalCharacter.speed * Time.deltaTime;
    //             animalCharacter.Move(position);
    //         }
    //         yield return null;
    //     }
    // }
    IEnumerator MoveAnimalAfterWait(Character animalCharacter)
    {
        yield return new WaitForSeconds(waitTimeAfterSpawn);

        while (animalCharacter != null && animalCharacter.gameObject != null && animalCharacter.gameObject.activeInHierarchy)
        {
            List<Character> enemies = gameManager.GetEnemies(animalCharacter);
            Character nearestEnemy = null;
            float shortestDistance = Mathf.Infinity;

            foreach (Character enemy in enemies)
            {
                // float distanceToEnemy = Vector3.Distance(animalCharacter.transform.position, enemy.transform.position);
                float distanceToEnemy = animalCharacter.agent.stoppingDistance;
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    Debug.Log("enemy found");
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null)
            {
                animalCharacter.SetTarget(nearestEnemy.transform);
                animalCharacter.agent.isStopped = false;
            }

            yield return null;
        }
    }

}
