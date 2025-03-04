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
    public Sprite BoarSprite;
    public Sprite PandaSprite;

    // public GameObject chickenPrefab;
    public GameObject dogPrefab;
    public GameObject horsePrefab;
    public GameObject eaglePrefab;
    public GameObject elephantPrefab;
    public GameObject slowtrapPrefab;
    public GameObject BoarPrefab;
    public GameObject PandaPrefab;

    private Dictionary<int, Sprite> animalSprites;
    private Dictionary<int, GameObject> animalPrefabs;
    private Dictionary<int, GameObject> spawnedAnimals = new Dictionary<int, GameObject>();

    void Start()
    {
        animalSprites = new Dictionary<int, Sprite>
        {
            //{ "Chicken", chickenSprite },
            { 0, dogSprite },
            { 1, horseSprite },
            { 2, eagleSprite },
            { 3, elephantSprite},
            { 4, slowtrapSprite},
            { 5, BoarSprite},
            { 6, PandaSprite}
        };

        animalPrefabs = new Dictionary<int, GameObject>
        {
            //{ "Chicken", chickenPrefab },
            { 0, dogPrefab },
            { 1, horsePrefab },
            { 2, eaglePrefab },
            { 3, elephantPrefab},
            { 4, slowtrapPrefab},
            { 5, BoarPrefab},
            { 6, PandaPrefab}
        };

        string selectedAnimalsString = PlayerPrefs.GetString("SelectedAnimals");
        string[] selectedAnimalString = selectedAnimalsString.Split(',');
        int[] selectedAnimals = System.Array.ConvertAll(selectedAnimalString, int.Parse);

        for (int i = 0; i < selectedAnimals.Length && i < animalImages.Length && i < dragDropScripts.Length; i++)
        {
            int animalIndex = selectedAnimals[i];

            if (animalSprites.ContainsKey(animalIndex) && animalPrefabs.ContainsKey(animalIndex))
            {
                animalImages[i].sprite = animalSprites[animalIndex]; // スロットの画像を設定
                dragDropScripts[i].prefabToSpawn = animalPrefabs[animalIndex]; // プレハブを設定
                animalImages[i].gameObject.SetActive(true); // スロットを表示
            }


        }
    }
}
