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
        //UpdateTargets();
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
        List<Character> enemies = new List<Character>();

        if (playerAnimals.Contains(requester))
        {
            enemies.AddRange(botAnimals);
        }
        else if (botAnimals.Contains(requester))
        {
            enemies.AddRange(playerAnimals);
        }

        return enemies.FindAll(e => e.team != requester.team);
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
