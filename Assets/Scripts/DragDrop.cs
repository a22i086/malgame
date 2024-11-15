using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabToSpawn;
    private Canvas canvas;
    private GameObject spawnedObject; // ドラッグ中のプレハブ
    private Rigidbody rb; // Rigidbody参照
    private float fixedY = 0.5f; // 固定するY座標

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // プレハブを生成してカーソルに追随させる
        Vector3 spawnPosition = GetWorldPosition(eventData);
        spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Rigidbodyを無効化
        rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (spawnedObject != null)
        {
            // プレハブの位置を更新
            Vector3 newPosition = GetWorldPosition(eventData);
            newPosition.y = fixedY; // Y座標を固定
            spawnedObject.transform.position = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (spawnedObject != null)
        {
            // ドラッグが終了した位置にプレハブを確定させる
            Vector3 finalPosition = GetWorldPosition(eventData);
            finalPosition.y = fixedY; // Y座標を固定
            spawnedObject.transform.position = finalPosition;

            // Rigidbodyを有効化
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            spawnedObject = null;
            Debug.Log($"Spawned {prefabToSpawn.name} at {finalPosition}");
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
}
