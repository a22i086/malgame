using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAlert : MonoBehaviour
{
    public GameObject alertIconPrefab; // ビックリマークのプレハブ
    public Camera mainCamera; // メインカメラ
    public List<Transform> enemies; // 監視する敵のリスト
    public Canvas uiCanvas; // UI Canvas への参照

    private List<GameObject> alertIcons; // ビックリマークのインスタンスリスト

    void Start()
    {
        alertIcons = new List<GameObject>();
        foreach (Transform enemy in enemies)
        {
            GameObject alertIcon = Instantiate(alertIconPrefab, uiCanvas.transform);
            alertIcon.SetActive(false);
            alertIcons.Add(alertIcon);
            // Debug.Log($"Created alert icon for {enemy.name}");
        }
    }

    void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Transform enemy = enemies[i];
            GameObject alertIcon = alertIcons[i];
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(enemy.position);

            if (IsOutOfViewport(viewportPosition))
            {
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemy.position);

                // スクリーンの範囲内に制限
                screenPosition.x = Mathf.Clamp(screenPosition.x, 50, Screen.width - 50);
                screenPosition.y = Mathf.Clamp(screenPosition.y, 50, Screen.height - 50);

                RectTransform rectTransform = alertIcon.GetComponent<RectTransform>();
                rectTransform.position = screenPosition;

                alertIcon.SetActive(true);
                // Debug.Log($"Showing alert icon at {screenPosition} for {enemy.name}");
            }
            else
            {
                alertIcon.SetActive(false);
            }
        }
    }

    bool IsOutOfViewport(Vector3 viewportPosition)
    {
        //viewportPositionのx座標やy座標が０未満または１を超える場合は画面外とみなす
        return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
    }
}
