using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour, ICharacter
{
    public float health;
    public float speed;

    public abstract void Attack();

    public virtual void Move(Vector3 targetPosition) //引数として移動先の座標を受け取り、非同期の移動処理をする
    {
        StartCoroutine(MoveCoroutine(targetPosition));
    }

    protected IEnumerator MoveCoroutine(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
