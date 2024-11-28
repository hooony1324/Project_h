using UnityEngine;

public class SleepingState : EntityCCState
{
    private static readonly int kAnimationHash = Animator.StringToHash("isSleeping");

    public override string Description => "Sleeping";
    protected override int AnimationHash => kAnimationHash;
}