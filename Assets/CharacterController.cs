using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask characterLayer;
    private float clickCooldown = 1.0f;
    private float lastClickTime;
    private GameObject selectedCharacter; //選択されたキャラクターを保持

    void OnEnable()
    {
        GameEvents.OnCharacterSelected += HandleCharacterSelected;
        GameEvents.OnMoveCommand += HandleMoveCommand;
    }

    void OnDisable()
    {
        GameEvents.OnCharacterSelected -= HandleCharacterSelected;
        GameEvents.OnMoveCommand -= HandleMoveCommand;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime > clickCooldown) // クリックの間に待ち時間を設ける
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, characterLayer)) //キャラクターに当たった時
                {
                    if (selectedCharacter == hit.collider.gameObject) //選択したキャラが同じだった時
                    {
                        selectedCharacter = null;
                        Debug.Log("選択を解除");
                    }
                    else
                    {
                        GameEvents.OnCharacterSelected?.Invoke(hit.collider.gameObject);
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)) //地面に当たった時
                {
                    GameEvents.OnMoveCommand?.Invoke(hit.point);
                }
                lastClickTime = Time.time; // 最後にクリックした時刻を更新
            }
        }
    }

    private void HandleCharacterSelected(GameObject character) //選択されたときに呼び出される この部分が怪しい(犬以外の挙動について)
    {
        if (Time.time - lastClickTime > clickCooldown)
        {
            selectedCharacter = character;
            Debug.Log($"{character.name} selected at {Time.time}");
        }
        lastClickTime = Time.time;
    }

    private void HandleMoveCommand(Vector3 targetPosition) //移動コマンド
    {
        if (selectedCharacter != null)
        {
            ICharacter character = selectedCharacter.GetComponent<ICharacter>();
            if (character != null)
            {
                character.Move(targetPosition);
            }
        }
    }
}