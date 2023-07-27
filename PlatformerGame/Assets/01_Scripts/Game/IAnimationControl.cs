using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationControl<in T> where T : Enum //in keyword is using when you use T for just input, not have to return T
{
    void Init();
    void Play(T motionEnum);
    void GetRunningTime(T motionEnum);
}
