using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_collide : MonoBehaviour
{
    public int blood = 10;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Player2_attack"));
        if (stateInfo.IsName("Player2_defend"));
    }
}
