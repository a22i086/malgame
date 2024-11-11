using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] LayerMask groundLayer; // 地面レイヤーを指定
    bool isMoving; //移動判定
    Vector3 targetPos;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // マウス座標からレイを飛ばす
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                targetPos = hit.point; //当たった地点をターゲット位置に設定
                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move()
    {
        isMoving = true; // 移動用フラグをtrue
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) // ワールド座標と自身の座標を比較しループ
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); //指定した座標に向かって移動

            yield return null; // 1フレーム待つ
        }
        isMoving = false; // 移動用フラグをfalse
    }
}
