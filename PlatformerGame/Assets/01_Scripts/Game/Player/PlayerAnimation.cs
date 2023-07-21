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

    int[] animIDs = new int[(int)PlayerAnimations.Max];
    int[] animRunningTimes = new int[(int)PlayerAnimations.Max];
    
    void Start()
    {
        var length = (int)PlayerAnimations.Max;
        var clips = animator.runtimeAnimatorController.animationClips;
        
        for (int i = 0; i < length; i++)
        {
            var animation = (PlayerAnimations)i;
            var id = Animator.StringToHash(animation.ToString());

            
            
            animIDs[i] = id;
        }

        //for checking
        for (int i = 0; i < length; i++)
        {
            Debug.Log(animIDs[i]);
        }
    }

    public void PlayAnimation(PlayerAnimations motion)
    {
        var indexOfId = (int)motion;
        if (animIDs.Length <= indexOfId) return;

        animator.SetTrigger(animIDs[indexOfId]);
    }

    void InsertClipRunningTimeIntoRunningTimeArray(string animName)
    {
        var length = (int)PlayerAnimations.Max;
        for (int i = 0; i < length; i++)
        {
            
        }
    }
}
