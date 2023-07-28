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

public class PlayerAnimation : AnimationControl
{
    public PlayerAnimation(Animator animator)
    {
        base.Init(animator, (int)PlayerAnimations.Max);
    }

    public void Play(PlayerAnimations motion)
    {
        base.Play((int)motion);
    }

    public float GetRunningTime(PlayerAnimations motion)
    {
        return base.GetRunningTime((int)motion);
    }
}
