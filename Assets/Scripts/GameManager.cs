using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public GameObject gameWinCanvas;
    public GameObject subTowerDestroyedCanvas;
    public SpawnRangeManager spawnRangeManager;
    private List<Character> playerAnimals = new List<Character>();
    private List<Character> botAnimals = new List<Character>();
    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.SetActive(false);
        gameWinCanvas.SetActive(false);
        subTowerDestroyedCanvas.SetActive(false);
        spawnRangeManager = FindObjectOfType<SpawnRangeManager>();
    }
    void Update()
    {
        // UpdateTargets();
    }

    public void AddPlayerAnimal(Character animal)
    {
        playerAnimals.Add(animal);
    }
    public void AddBotAnimal(Character animal)
    {
        botAnimals.Add(animal);
    }

    public List<Character> GetEnemies(Character requester)
    {

        List<Character> enemies = new List<Character>();

        if (playerAnimals.Contains(requester))
        {
            enemies.AddRange(botAnimals);
        }
        else if (botAnimals.Contains(requester))
        {
            enemies.AddRange(playerAnimals);
        }

        List<Character> filteredEnemies = enemies.FindAll(e => e != null && e.gameObject != null
                                                            && e.team != requester.team);
        return filteredEnemies;
    }

    public List<Character> GetEnemiesForTower(Tower tower)
    {
        List<Character> enemies = new List<Character>();

        if (tower.team == 0)
        {
            enemies.AddRange(botAnimals);
        }
        else if (tower.team == 1)
        {
            enemies.AddRange(playerAnimals);
        }

        List<Character> filteredEnemies = enemies.FindAll(e => e != null && e.gameObject != null && e.gameObject.activeInHierarchy
                                                        && e.team != tower.team);

        return filteredEnemies;
    }

    public List<Character> GetAllies(Character requester)
    {
        List<Character> allies = new List<Character>();

        if (playerAnimals.Contains(requester))
        {
            allies.AddRange(playerAnimals);
        }
        else if (botAnimals.Contains(requester))
        {
            allies.AddRange(botAnimals);
        }
        List<Character> filteredAllies = allies.FindAll(a => a != null && a.gameObject != null && a.gameObject.activeInHierarchy
                                                        && a.team == requester.team && a != requester);
        return filteredAllies;
    }
    public void RemoveAnimal(Character animal)
    {
        if (playerAnimals.Contains(animal))
        {
            playerAnimals.Remove(animal);
        }
        else if (botAnimals.Contains(animal))
        {
            botAnimals.Remove(animal);
        }
    }
    public void Game_Win()
    {
        gameWinCanvas.SetActive(true);
        PlayerPrefs.DeleteAll();
        spawnRangeManager.ResetSpawnRange();
        StartCoroutine(StopGameAfterAnimation());
    }
    public void Game_Over()
    {
        gameOverCanvas.SetActive(true);
        PlayerPrefs.DeleteAll();
        spawnRangeManager.ResetSpawnRange();
        StartCoroutine(StopGameAfterAnimation());
    }
    public void SubTower_Destroy()
    {
        StartCoroutine(ClosedDestroyMS());
    }

    private IEnumerator StopGameAfterAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0;
    }
    private IEnumerator ClosedDestroyMS()
    {
        subTowerDestroyedCanvas.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        subTowerDestroyedCanvas.SetActive(false);
    }

    public void RestartGame()
    {
        gameOverCanvas.SetActive(false);
        gameWinCanvas.SetActive(false);

        Time.timeScale = 1;
    }

    // internal void AddBotAnimal(AnimalController animalController)
    // {
    //     throw new NotImplementedException();
    // }
}
