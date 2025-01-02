

using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class RushAction : SkillAction
{
    readonly float _rushRate = 0.8f;

    public override void Apply(Skill skill)
    {
        var sourcePos = skill.Owner.transform.position;
        var targetPos = skill.TargetSelectionResult.selectedPosition; // -> 부채꼴 가운데 방향으로

        var searchProperRange = skill.TargetSearcher.SearchProperRange;
        var rushDistance = searchProperRange is float ? (float)searchProperRange : ((Vector2)searchProperRange).y;
        var destPos = sourcePos + (targetPos - sourcePos).normalized * rushDistance * _rushRate;
        destPos.With(z:0);

        DOTween.To(() => skill.Owner.transform.position, (v) => skill.Owner.transform.position = v, destPos, 0.5f)
            .SetEase(Ease.OutExpo);

        skill.SearchTargets();
        foreach (var target in skill.Targets)
            target.SkillSystem.Apply(skill);
    }

    public override object Clone() => new RushAction();
}
