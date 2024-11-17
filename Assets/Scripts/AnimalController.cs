using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AnimalController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask characterLayer;
    private NavMeshAgent agent;
    private bool isSelected = false;
    private bool canMove = true;
    private float holdDuration = 0.5f; // 長押し時間（秒）
    private float holdTime = 0.0f;
    private ICharacter character; //キャラクターのインターフェイス

    void Start()
    {
        character = GetComponent<ICharacter>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;

            if (holdTime >= holdDuration)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // キャラクターレイヤーにヒットしたかどうかのログを出力
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, characterLayer))
                {
                    Debug.Log("Character Layer Hit: " + hit.transform.name);
                    // 動物がクリックされたかどうかをチェック
                    if (hit.transform == transform)
                    {
                        isSelected = true;
                        Debug.Log("Selected: " + isSelected);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isSelected && canMove)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    Debug.Log("Ground Layer Hit: " + hit.point);
                    character.Move(hit.point); //キャラクターのMoveを呼び出す
                }

                // クールダウンを開始
                StartCoroutine(Cooldown());
            }

            // 選択状態をリセット
            holdTime = 0.0f;
            isSelected = false;
        }
    }

    private IEnumerator Cooldown()
    {
        canMove = false;  // 移動を一時的に無効化
        yield return new WaitForSeconds(1.0f);  // クールダウン時間待機
        canMove = true;  // 再び移動を有効化
    }
}
