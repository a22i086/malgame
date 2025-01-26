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
        //spawnPosition.z -= 3.0f;
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

    }


}
