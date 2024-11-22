using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    private Animator myAnimator;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
}
