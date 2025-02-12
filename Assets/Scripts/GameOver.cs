using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void RetryCurrentScene()
    {
        SceneManager.LoadScene("AnimalSelectionScene");
        gameManager.RestartGame();
    }
    public void BackToTitleScene()
    {
        SceneManager.LoadScene("StartScene");
        gameManager.RestartGame();
    }
}
