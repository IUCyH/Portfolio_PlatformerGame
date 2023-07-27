using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationControl : MonoBehaviour
{
    
    
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
