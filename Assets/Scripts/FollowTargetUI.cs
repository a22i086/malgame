using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetUI : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Vector3 offset;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットに追うUI
        if (target != null)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(target.position + offset);
            rectTransform.position = screenPosition;
        }
    }
}
