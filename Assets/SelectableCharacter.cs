using UnityEngine;
namespace YourNamespace
{

    public class SelectableCharacter : MonoBehaviour
    {
        private bool isSelected;

        private void OnMouseDown()
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                // 選択されたキャラクターの色を変更するなどの視覚的なフィードバックを追加
                GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                // 選択が解除された場合の処理
                GetComponent<Renderer>().material.color = Color.white;
            }
        }

        public bool IsSelected()
        {
            return isSelected;
        }
    }
}