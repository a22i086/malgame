using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    private List<Character> playerAnimals = new List<Character>();
    private List<Character> botAnimals = new List<Character>();
    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.SetActive(false);
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

    // private void UpdateTargets()
    // {
    //     foreach (var playerAnimal in playerAnimals)
    //     {
    //         if (botAnimals.Count > 0)
    //         {
    //             playerAnimal.SetTarget(botAnimals[0].transform);
    //         }
    //     }
    //     foreach (var botAnimal in botAnimals)
    //     {
    //         if (playerAnimals.Count > 0)
    //         {
    //             botAnimal.SetTarget(playerAnimals[0].transform);
    //         }
    //     }
    // }

    public List<Character> GetEnemies(Character requester)
    {
        try
        {


            Debug.Log("GetEnemies called for: " + requester.name);

            List<Character> enemies = new List<Character>();

            if (playerAnimals.Contains(requester))
            {
                enemies.AddRange(botAnimals);
            }
            else if (botAnimals.Contains(requester))
            {
                enemies.AddRange(playerAnimals);
            }

            // フィルタリング前の敵リストを表示
            Debug.Log("Before filtering: ");
            foreach (Character enemy in enemies)
            {
                Debug.Log("Found enemy: " + enemy.name + " (Team: " + enemy.team + ")");
            }
            // チーム番号でフィルタリング
            List<Character> filteredEnemies = enemies.FindAll(e => e != null && e.gameObject != null && e.team != requester.team);
            // フィルタリング後の敵リストを表示
            Debug.Log("After filtering: ");
            foreach (Character enemy in filteredEnemies)
            {
                Debug.Log("Filtered enemy: " + enemy.name + " (Team: " + enemy.team + ")");
            }
            return filteredEnemies;
        }
        catch (Exception e)
        {
            Debug.LogError("Error in GetEnemies" + e.Message);
            return new List<Character>();
        }
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
    public void Game_Over()
    {
        gameOverCanvas.SetActive(true);
    }

    // internal void AddBotAnimal(AnimalController animalController)
    // {
    //     throw new NotImplementedException();
    // }
}
