using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public List<GameObject> animalPrefabs; // 召喚する動物のプレハブ
    public Transform spawnPoint; // 動物を召喚する位置
    public float spawnInterval = 5f; // 召喚の間隔
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
        GameObject animalInstance = Instantiate(selectedAnimalPrefab, spawnPoint.position, Quaternion.identity);
        Character animalCharacter = animalInstance.GetComponent<Character>();
        animalCharacter.team = 1;
        gameManager.AddBotAnimal(animalCharacter);
    }
}
