using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YourNamespace; // SelectableCharacterクラスが含まれている名前空間を追加


public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] LayerMask groundLayer; // 地面レイヤーを指定
    [SerializeField] LayerMask characterLayer; // キャラクターレイヤーを指定
    bool isMoving; //移動判定
    Vector3 targetPos;
    List<GameObject> selectedCharacters = new List<GameObject>(); // 選択されたキャラクターを管理するリスト
    //GameObject selectedCharacter; // 選択されたキャラクターを格納
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, characterLayer))
            {
                GameObject clickedCharacter = hit.collider.gameObject;
                if (!selectedCharacters.Contains(clickedCharacter))
                {
                    selectedCharacters.Add(clickedCharacter);
                    Debug.Log("選択されたよ");
                }
                // selectedCharacter = hit.collider.gameObject; // 選択されたキャラクターを設定

            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                targetPos = hit.point; //当たった地点をターゲット位置に設定
                StartCoroutine(MoveSelectedCharacters());
            }
        }
    }
    IEnumerator MoveSelectedCharacters()
    {
        isMoving = true; // 移動フラグをtrueに設定
        // SelectableCharacter[] characters = FindObjectsOfType<SelectableCharacter>();
        // foreach (var character in characters)
        // {
        //     if (character.IsSelected())
        //     {
        //         StartCoroutine(MoveCharacter(character.gameObject)); // 各キャラクターの移動を開始
        //         Debug.Log("キャラが移動するよ");
        //     }
        // }
        //yield return new WaitUntil(() => !characters.Any(character => character.IsSelected() && Vector3.Distance(character.transform.position, targetPos) > Mathf.Epsilon));
        //isMoving = false; // 移動フラグをfalseに設定

        foreach (var character in selectedCharacters)
        {
            StartCoroutine(MoveCharacter(character));
            Debug.Log("キャラが移動するよ");
        }
        yield return new WaitUntil(() => !selectedCharacters.Any(character => Vector3.Distance(character.transform.position, targetPos) <= Mathf.Epsilon));
        isMoving = false; // 移動フラグをfalseに設定
    }

    IEnumerator MoveCharacter(GameObject character)
    {
        //isMoving = true; // 移動用フラグをtrue
        while ((targetPos - character.transform.position).sqrMagnitude > Mathf.Epsilon) // ワールド座標と自身の座標を比較しループ
        {
            character.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); //指定した座標に向かって移動

            yield return null; // 1フレーム待つ
        }

    }
}
