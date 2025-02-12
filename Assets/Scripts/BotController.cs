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
    private List<GameObject> selectedAnimals = new List<GameObject>();

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemyAlert = FindObjectOfType<EnemyAlert>();

        SelectRandomAnimals();
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

    void SelectRandomAnimals()
    {
        List<GameObject> shuffledAnimals = new List<GameObject>(animalPrefabs);
        for (int i = 0; i < shuffledAnimals.Count; i++)
        {
            int rnd = Random.Range(0, shuffledAnimals.Count);
            GameObject temp = shuffledAnimals[i];
            shuffledAnimals[i] = shuffledAnimals[rnd];
            shuffledAnimals[rnd] = temp;
        }

        selectedAnimals = shuffledAnimals.GetRange(0, Mathf.Min(3, shuffledAnimals.Count));
    }

    void SpawnAnimal()
    {
        GameObject selectedAnimalPrefab = selectedAnimals[Random.Range(0, selectedAnimals.Count)];
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
