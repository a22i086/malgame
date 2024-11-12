using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCharacter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject characterPrefab; // キャラクタープレハブ
    private GameObject draggedCharacter;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグを開始したときにキャラクターを生成
        draggedCharacter = Instantiate(characterPrefab, transform.position, Quaternion.identity);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ中にキャラクターをマウス位置に合わせて移動
        if (draggedCharacter != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
            draggedCharacter.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ドラッグを終了したときの処理
        if (draggedCharacter != null)
        {
            // キャラクターの位置をステージに合わせて調整
            draggedCharacter.transform.position = new Vector3(draggedCharacter.transform.position.x, draggedCharacter.transform.position.y, 0);

            // 必要ならば、他の初期設定をここで行う

            draggedCharacter = null;
        }
    }
}
