using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour, ICharacter
{
    private Vector3 initialPosition;
    private NavMeshAgent agent;

    void Awake()
    {
        initialPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false; // NavMeshAgentの高さ制御を無効化
    }

    public void Move(Vector3 targetPosition)
    {
        Vector3 fixedPosition = new Vector3(targetPosition.x, initialPosition.y, targetPosition.z);
        agent.SetDestination(fixedPosition);
        StartCoroutine(UpdatePosition());
    }
    private IEnumerator UpdatePosition() //Y座標を固定したまま移動制御
    {
        /*agent.pathPendingはエージェントが新しい経路を計算している間trueになる。
          agent.remainingDistanceはエージェントが目的地までの残り距離を返す。
          agent.stoppingDistanceはエージェントが目的地に到達とみなす距離を示す。*/
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 position = agent.nextPosition; // エージェントの次のフレームでの予測位置をpositionに代入
            position.y = initialPosition.y;
            agent.transform.position = position;
            yield return null;
        }
    }

    public void Attack()
    {
        Debug.Log("Chicken is attacking!");
    }
}