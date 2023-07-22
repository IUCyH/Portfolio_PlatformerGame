using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimations
{
    Move,
    Hit,
    Idle,
    Attack,
    Die,
    Max
}

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    int[] animIDs = new int[(int)PlayerAnimations.Max];
    float[] animRunningTimes = new float[(int)PlayerAnimations.Max];

    int prevMotion;
    
    void Start()
    {
        var length = (int)PlayerAnimations.Max;
        var clips = animator.runtimeAnimatorController.animationClips;
        
        for (int i = 0; i < length; i++)
        {
            var motion = (PlayerAnimations)i;
            var id = Animator.StringToHash(motion.ToString());
            
            animIDs[i] = id;
            animRunningTimes[i] = clips[i].length;
        }

        prevMotion = animIDs[(int)PlayerAnimations.Idle];
        //for checking
        for (int i = 0; i < length; i++)
        {
            Debug.Log(animIDs[i]);
            Debug.Log(animRunningTimes[i]);
        }
    }

    public void PlayAnimation(PlayerAnimations motion)
    {
        var animId = animIDs[(int)motion];
        
        animator.ResetTrigger(prevMotion);
        animator.SetTrigger(animId);
        
        prevMotion = animId;
    }

    public float GetAnimationRunningTime(PlayerAnimations motion)
    {
        return animRunningTimes[(int)motion];
    }
}
