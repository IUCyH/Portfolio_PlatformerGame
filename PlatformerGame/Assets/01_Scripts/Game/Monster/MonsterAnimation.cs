using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMotions
{
    Move,
    Hit,
    Idle,
    Attack,
    Die,
    Max
}

public class MonsterAnimation : AnimationControl
{
    public MonsterAnimation(Animator animator)
    {
        base.Init(animator, (int)MonsterMotions.Max);
    }

    public void Play(MonsterMotions motion)
    {
        base.Play((int)motion);
    }

    public float GetRunningTime(MonsterMotions motion)
    {
        return base.GetRunningTime((int)motion);
    }
}
