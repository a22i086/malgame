using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public List<GameObject> animalPrefabs; // 召喚する動物のプレハブ
    public Transform spawnPoint; // 動物を召喚する位置
    public float spawnInterval = 5f; // 召喚の間隔
    public float waitTimeAfterSpawn = 2f;
    private float spawnTimer = 0f;
    public GameManager gameManager; // ゲームマネージャーへの参照

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        GameObject animalInstance = Instantiate(selectedAnimalPrefab, spawnPoint.position, Quaternion.identity);
        Character animalCharacter = animalInstance.GetComponent<Character>();
        animalCharacter.team = 1;
        animalCharacter.isPlayerControlled = false;
        animalCharacter.isSpawnConfirmed = true;
        gameManager.AddBotAnimal(animalCharacter);

        StartCoroutine(MoveAnimalAfterWait(animalCharacter));
    }

    IEnumerator MoveAnimalAfterWait(Character animalCharacter)
    {
        yield return new WaitForSeconds(waitTimeAfterSpawn);

        while (animalCharacter != null && animalCharacter.gameObject != null && animalCharacter.gameObject.activeInHierarchy)
        {
            List<Character> enemies = gameManager.GetEnemies(animalCharacter);
            bool enemyInRange = false;

            foreach (Character enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(animalCharacter.transform.position,
                                                            enemy.transform.position);
                if (distanceToEnemy <= animalCharacter.attackRange)
                {
                    enemyInRange = true;
                    break;
                }
            }

            if (!enemyInRange)
            {
                Vector3 position = animalCharacter.transform.position;
                position.z -= animalCharacter.speed * Time.deltaTime;
                animalCharacter.Move(position);
            }
            yield return null;
        }
    }
}
