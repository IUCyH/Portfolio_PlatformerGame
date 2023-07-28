using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl
{
    Animator animator;
    int[] animIDs;
    float[] animRunningTimes;

    int prevMotion;

    protected void Init(Animator animator, int numberOfMotions)
    {
        Debug.Log("Name : " + this + " " + "motions : " + numberOfMotions);
        this.animator = animator;
        animIDs = new int[numberOfMotions];
        animRunningTimes = new float[numberOfMotions];
        
        var length = numberOfMotions;
        var clips = animator.runtimeAnimatorController.animationClips;
        
        for (int i = 0; i < length; i++)
        {
            var clip = clips[i];
            var id = Animator.StringToHash(clip.name);
            
            animIDs[i] = id;
            animRunningTimes[i] = clip.length;
        }

        prevMotion = animIDs[0];
        
        Debug.Log(this);
        //for checking
        for (int i = 0; i < length; i++)
        {
            Debug.Log(clips[i].name);
            Debug.Log(animIDs[i]);
            Debug.Log(animRunningTimes[i]);
        }
    }

    protected void Play(int motion)
    {
        var animId = animIDs[motion];
        
        animator.ResetTrigger(prevMotion);
        animator.SetTrigger(animId);
        
        prevMotion = animId;
    }

    protected float GetRunningTime(int motion)
    {
        return animRunningTimes[motion];
    }
}
