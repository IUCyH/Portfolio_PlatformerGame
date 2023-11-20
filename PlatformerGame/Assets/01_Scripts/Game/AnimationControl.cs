using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl
{
    Animator animator;
    string[] animNames;
    float[] animRunningTimes;

    string prevMotion;

    protected void Init(Animator animator, int numberOfMotions)
    {
        this.animator = animator;
        animNames = new string[numberOfMotions];
        animRunningTimes = new float[numberOfMotions];
        
        var length = numberOfMotions;
        var clips = animator.runtimeAnimatorController.animationClips;
        
        for (int i = 0; i < length; i++)
        {
            var clip = clips[i];
            animNames[i] = clip.name;
            animRunningTimes[i] = clip.length;
        }

        prevMotion = animNames[0];
    }

    protected void Play(int motion)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animNames[motion])) return;
        
        var animName = animNames[motion];
        
        animator.ResetTrigger(prevMotion);
        animator.SetTrigger(animName);
        
        prevMotion = animName;
    }

    protected float GetRunningTime(int motion)
    {
        return animRunningTimes[motion];
    }
}
