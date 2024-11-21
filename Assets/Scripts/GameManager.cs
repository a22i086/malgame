using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    // Start is called before the first frame update
    void Start()
    {
        gameOverCanvas.SetActive(false);
    }
    public void Game_Over()
    {
        gameOverCanvas.SetActive(true);
    }
}
