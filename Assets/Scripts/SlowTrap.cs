using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    public float slowAmount = 0.1f; // 減速率（0.5 は速度を半分にします）
    public float slowDuration = 5f; // 減速時間（秒）

    void Start()
    {
        StartCoroutine(AfterSpawned());
    }

    void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null && character.team == 1)
        {
            StartCoroutine(SlowCharacter(character));
        }
    }

    IEnumerator SlowCharacter(Character character)
    {
        Debug.Log("" + character.speed);
        float originalSpeed = character.speed;
        character.speed *= slowAmount;
        character.agent.speed = character.speed; // NavMeshAgentの速度も変更
        Debug.Log("" + character.agent.speed);

        yield return new WaitForSeconds(slowDuration);

        character.speed = originalSpeed;
        character.agent.speed = character.speed; // NavMeshAgentの速度も元に戻す
    }

    IEnumerator AfterSpawned()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
