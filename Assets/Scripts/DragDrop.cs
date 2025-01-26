using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabToSpawn;
    public float cooldownTime;
    private float lastSpawnTime;
    private Canvas canvas;
    private GameObject spawnedObject; // ドラッグ中のプレハブ
    private Rigidbody rb; // Rigidbody参照
    private float fixedY = 0.5f; // 固定するY座標
    public GameObject cooldownCircle; // GameObject に変更
    public GameObject SpawnRangeLimit;
    public GameObject outOfBoundsIndicator;
    public GameManager gameManager; // ゲームマネージャーへの参照
    public float spawnRange = 5f;

    // ステージの境界を定義
    public Vector3 stageMinBounds = new Vector3(-7f, -2f, -52f); // 召喚できる最小
    public Vector3 stageMaxBounds = new Vector3(13f, 10f, -12f); // サブ塔が壊されたとき変化する
    public float reduceMaxBound = 25f;

    public Camera mainCamera;


    void Awake()
    {
        SpawnRangeLimit.SetActive(false);
        outOfBoundsIndicator.SetActive(false);
        canvas = GetComponentInParent<Canvas>();
        lastSpawnTime = -cooldownTime;
        gameManager = FindObjectOfType<GameManager>();
        if (cooldownCircle != null)
        {
            cooldownCircle.SetActive(false); // 初期状態で非表示
        }
    }

    void Update()
    {
        if (cooldownCircle != null)
        {
            float elapsedTime = Time.time - lastSpawnTime;
            float fillAmount = Mathf.Clamp01(elapsedTime / cooldownTime);
            Image cooldownImage = cooldownCircle.GetComponent<Image>();
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = fillAmount;
            }

            // クールダウン中は円を表示し、終了後は非表示にする
            if (fillAmount < 1f)
            {
                cooldownCircle.SetActive(true);
            }
            else
            {
                cooldownCircle.SetActive(false);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (Time.time >= lastSpawnTime + cooldownTime)
        {
            // プレハブを生成してカーソルに追随させる
            Vector3 spawnPosition = GetWorldPosition(eventData);
            Debug.Log("スポーン範囲" + stageMaxBounds);
            Debug.Log("スポーン地点=" + spawnPosition);
            spawnPosition = ClampToSpawnRange(spawnPosition);
            if (IsInView(spawnPosition))
            {
                spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                Character animalCharacter = spawnedObject.GetComponent<Character>();
                animalCharacter.team = 0;
                animalCharacter.isPlayerControlled = true;
                animalCharacter.isSpawnConfirmed = false;
                gameManager.AddPlayerAnimal(animalCharacter);

                // Rigidbodyを無効化
                rb = spawnedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }

            else
            {
                Debug.Log("Spawn position is out of camera view");
                StartCoroutine(ClosedErrorMS(spawnPosition));
            }

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (spawnedObject != null)
        {
            Character animalCharacter = spawnedObject.GetComponent<Character>();
            if (animalCharacter.isPlayerControlled)
            {
                // プレハブの位置を更新
                Vector3 newPosition = GetWorldPosition(eventData);
                newPosition = ClampToSpawnRange(newPosition);
                newPosition.y = fixedY; // Y座標を固定

                if (IsInView(newPosition))
                {
                    spawnedObject.transform.position = newPosition;
                }
                else
                {
                    Debug.Log("Drag position is out of camera view");
                }
            }

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (spawnedObject != null)
        {
            Character animalCharacter = spawnedObject.GetComponent<Character>();
            if (animalCharacter.isPlayerControlled)
            {
                // ドラッグが終了した位置にプレハブを確定させる
                Vector3 finalPosition = GetWorldPosition(eventData);
                finalPosition = ClampToSpawnRange(finalPosition);
                finalPosition.y = fixedY; // Y座標を固定

                spawnedObject.transform.position = finalPosition;

                // Rigidbodyを有効化
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                animalCharacter.isSpawnConfirmed = true;
                spawnedObject = null;
                Debug.Log($"Spawned {prefabToSpawn.name} at {finalPosition}");
                lastSpawnTime = Time.time;
                SpawnRangeLimit.SetActive(false);
                //動物を置いたらCT開始
                if (cooldownCircle != null)
                {
                    Image cooldownImage = cooldownCircle.GetComponent<Image>();
                    if (cooldownImage != null)
                    {
                        cooldownImage.fillAmount = 0f;
                    }
                    cooldownCircle.SetActive(true);
                }
            }

        }
    }

    private Vector3 GetWorldPosition(PointerEventData eventData)
    {
        // スクリーン座標をワールド座標に変換
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPosition = hit.point;
            return worldPosition;
        }
        // 万が一レイキャストがヒットしなかった場合、スクリーン座標からワールド座標に変換して適切なY座標を設定
        Vector3 defaultPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.transform.position.y - fixedY));
        defaultPosition.y = fixedY; // Y座標を固定
        return defaultPosition;
    }
    public void OnSubTowerDestroyed()
    {
        stageMaxBounds.z -= reduceMaxBound;
        Debug.Log("新しいスポーン範囲" + stageMaxBounds);
        Debug.Log("スポーン範囲が狭まった！");
    }

    private Vector3 ClampToSpawnRange(Vector3 position)
    {
        // position.x = Mathf.Clamp(position.x, stageMinBounds.x, stageMaxBounds.x);
        // position.z = Mathf.Clamp(position.z, stageMinBounds.z, stageMaxBounds.z);

        Vector3 clampedPosition = new Vector3(
       Mathf.Clamp(position.x, stageMinBounds.x, stageMaxBounds.x),
       position.y,
       Mathf.Clamp(position.z, stageMinBounds.z, stageMaxBounds.z)
   );

        return clampedPosition;
    }

    private bool IsInView(Vector3 position)
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(position);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

    private IEnumerator ClosedErrorMS(Vector3 position)
    {
        Vector3 clampedPosition = ClampToSpawnRange(position);
        clampedPosition += new Vector3(0f, 11f, -34f);
        outOfBoundsIndicator.transform.position = clampedPosition;
        SpawnRangeLimit.SetActive(true);
        outOfBoundsIndicator.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        outOfBoundsIndicator.SetActive(false);
        SpawnRangeLimit.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((stageMinBounds + stageMaxBounds) / 2, new Vector3(stageMaxBounds.x - stageMinBounds.x, 0, stageMaxBounds.z - stageMinBounds.z));
    }
}
