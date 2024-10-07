using UnityEngine;

public class StunningState : EntityCCState
{
    private static readonly int kAnimationHash = Animator.StringToHash("isStunning");

    public override string Description => "±âÀý";
    protected override int AnimationHash => kAnimationHash;
}
