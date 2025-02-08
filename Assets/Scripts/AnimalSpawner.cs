using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnimalSpawner : MonoBehaviour
{
    public Image[] animalImages; // スロット用のイメージ配列
    public DragDrop[] dragDropScripts; // DragDropスクリプトの配列

    // public Sprite chickenSprite;
    public Sprite dogSprite;
    public Sprite horseSprite;
    public Sprite eagleSprite;
    public Sprite elephantSprite;
    public Sprite slowtrapSprite;

    // public GameObject chickenPrefab;
    public GameObject dogPrefab;
    public GameObject horsePrefab;
    public GameObject eaglePrefab;
    public GameObject elephantPrefab;
    public GameObject slowtrapPrefab;

    private Dictionary<string, Sprite> animalSprites;
    private Dictionary<string, GameObject> animalPrefabs;

    void Start()
    {
        animalSprites = new Dictionary<string, Sprite>
        {
            //{ "Chicken", chickenSprite },
            { "犬", dogSprite },
            { "馬", horseSprite },
            { "鷲", eagleSprite },
            { "象", elephantSprite},
            { "蜘蛛", slowtrapSprite}
        };

        animalPrefabs = new Dictionary<string, GameObject>
        {
            //{ "Chicken", chickenPrefab },
            { "犬", dogPrefab },
            { "馬", horsePrefab },
            { "鷲", eaglePrefab },
            { "象", elephantPrefab},
            {"蜘蛛", slowtrapPrefab}
        };

        string selectedAnimalsString = PlayerPrefs.GetString("SelectedAnimals");
        string[] selectedAnimals = selectedAnimalsString.Split(',');

        for (int i = 0; i < selectedAnimals.Length && i < animalImages.Length && i < dragDropScripts.Length; i++)
        {
            string animalName = selectedAnimals[i];
            if (animalSprites.ContainsKey(animalName) && animalPrefabs.ContainsKey(animalName))
            {
                animalImages[i].sprite = animalSprites[animalName]; // スロットの画像を設定
                dragDropScripts[i].prefabToSpawn = animalPrefabs[animalName]; // プレハブを設定
                animalImages[i].gameObject.SetActive(true); // スロットを表示
            }
        }
    }
}
