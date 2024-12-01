using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ThrowFruits : MonoBehaviour
{
    public List<GameObject> FruitList;
    public float throwForce = 10f;
    public Transform spawnPoint;
    public int maxFruits = 10;

    private Queue<GameObject> spawnedFruits = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowRandomFruit();
        }
    }

    void ThrowRandomFruit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject selectedFruit = FruitList[Random.Range(0, FruitList.Count)]; // ランダムな果物を選択
            GameObject fruitInstance = Instantiate(selectedFruit, spawnPoint.position, Quaternion.identity); // 選ばれた果物を生成

            Vector3 direction = (hit.point - spawnPoint.position).normalized;
            fruitInstance.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse); // クリックした地点に向かって投げる

            spawnedFruits.Enqueue(fruitInstance); // キューに生成された果物を追加
            if (spawnedFruits.Count > maxFruits) // maxFruitsを超えたら古いやつから削除
            {
                GameObject oldFruit = spawnedFruits.Dequeue();
                Destroy(oldFruit);
            }
        }
    }
}
