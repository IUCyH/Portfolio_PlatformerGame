using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimations
{
    Idle,
    Move,
    Attack,
    Hit,
    Die,
    Max
}

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    int[] animID = new int[(int)PlayerAnimations.Max];

    public void PlayAnimation(PlayerAnimations motion)
    {
        var indexOfId = (int)motion;
        if (animID.Length <= indexOfId) return;

        animator.SetTrigger(animID[indexOfId]);
    }

    void Start()
    {
        var length = animID.Length;
        for (int i = 0; i < length; i++)
        {
            var animation = (PlayerAnimations)i;
            var id = Animator.StringToHash(animation.ToString());

            animID[i] = id;
        }

        for (int i = 0; i < length; i++)
        {
            Debug.Log(animID[i]);
        }
    }
}
