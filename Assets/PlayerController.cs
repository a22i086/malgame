using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    bool isMoving; //移動判定
    Vector3 mousePos, worldPos;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("test");
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) //移動中なら処理を受け付けない
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            mousePos = Input.mousePosition; //マウスの座標
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f)); //スクリーン座標をワールド座標に変換
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        isMoving = true; // 移動用フラグをtrue
        while ((worldPos - transform.position).sqrMagnitude > Mathf.Epsilon) // ワールド座標と自身の座標を比較しループ
        {
            transform.position = Vector3.MoveTowards(transform.position, worldPos, speed * Time.deltaTime); //指定した座標に向かって移動

            yield return null; // 1フレーム待つ
        }
        isMoving = false; // 移動用フラグをfalse
    }
}
