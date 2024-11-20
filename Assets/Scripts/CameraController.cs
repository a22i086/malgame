using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; // カメラの移動速度
    private Vector3 initialPosition;

    void Start()
    {
        // カメラの初期位置を保存
        initialPosition = transform.position;
    }

    void Update()
    {
        // 現在のカメラ位置を取得
        Vector3 pos = transform.position;

        if (Input.GetMouseButton(1)) // 右クリックを押している間
        {
            float move = panSpeed * Time.deltaTime;

            // Z方向にパン
            if (Input.GetAxis("Mouse Y") < 0) // 前
            {
                pos.z += move;
            }
            if (Input.GetAxis("Mouse Y") > 0) // 後ろ
            {
                pos.z -= move;
            }
        }

        // 高さを維持
        pos.y = initialPosition.y;
        transform.position = pos;
    }
}
