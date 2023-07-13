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
    int[] animId = new int[(int)PlayerAnimations.Max];

    public void PlayAnimation(PlayerAnimations motion)
    {
        var indexOfId = (int)motion;
        if (animId.Length <= indexOfId) return;
        
        animator.SetTrigger(animId[indexOfId]);
    }

    void Start()
    {
        var length = animId.Length;
        for (int i = 0; i < length; i++)
        {
            var animation = (PlayerAnimations)i;
            var id = Animator.StringToHash(animation.ToString());
            
            animId[i] = id;
        }

        for (int i = 0; i < length; i++)
        {
            Debug.Log(animId[i]);
        }
    }
}
