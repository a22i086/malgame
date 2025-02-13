using UnityEngine;
using System.Collections.Generic;

public class AnimalSelector : MonoBehaviour
{
    public List<GameObject> animalPrefabs; // 動物のプレハブリスト
    public List<GameObject> selectedAnimals = new List<GameObject>();
    public int maxAnimals = 3;
    public GameObject confirmPanel;
    public Transform[] spawnPositions; // スポーン位置の配列
    private int currentSpawnIndex = 0;

    private Dictionary<GameObject, GameObject> spawnedAnimals = new Dictionary<GameObject, GameObject>();


    void Start()
    {
        confirmPanel.SetActive(false);
    }

    public void SelectAnimal(int animalIndex)
    {
        GameObject animalPrefab = animalPrefabs[animalIndex];
        if (selectedAnimals.Contains(animalPrefab))
        {
            selectedAnimals.Remove(animalPrefab);
        }
        else if (selectedAnimals.Count < maxAnimals)
        {
            selectedAnimals.Add(animalPrefab);
        }

        if (selectedAnimals.Count == maxAnimals)
        {
            confirmPanel.SetActive(true);
        }
        else
        {
            confirmPanel.SetActive(false);
        }

        if (spawnedAnimals.ContainsKey(animalPrefab))
        {
            GameObject spawnedAnimal = spawnedAnimals[animalPrefab];
            if (spawnedAnimal != null)
            {
                Destroy(spawnedAnimal);
            }
            spawnedAnimals.Remove(animalPrefab);
            currentSpawnIndex--;
        }
        else
        {
            Vector3 spawnPosition = spawnPositions[currentSpawnIndex % spawnPositions.Length].position;
            Quaternion rotation = Quaternion.Euler(0, 180, 0);
            GameObject spawnedAnimal = Instantiate(animalPrefab, spawnPosition, rotation);
            spawnedAnimals.Add(animalPrefab, spawnedAnimal);

            currentSpawnIndex++;
        }
    }

    public void OnConfirm()
    {
        List<int> selectedAnimalIndexes = new List<int>();
        foreach (GameObject animalPrefab in selectedAnimals)
        {
            int index = animalPrefabs.IndexOf(animalPrefab);
            if (index != -1)
            {
                selectedAnimalIndexes.Add(index);
            }
        }
        PlayerPrefs.SetString("SelectedAnimals", string.Join(",", selectedAnimalIndexes));
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OnCancel()
    {
        selectedAnimals.Clear();
        currentSpawnIndex = 0;
        confirmPanel.SetActive(false);

        foreach (var spawnedAnimal in spawnedAnimals.Values)
        {
            Destroy(spawnedAnimal);
        }
        spawnedAnimals.Clear();
    }
}
