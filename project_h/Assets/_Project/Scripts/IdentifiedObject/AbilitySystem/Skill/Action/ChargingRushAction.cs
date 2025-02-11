

using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ChargingRushAction : SkillAction
{
    [SerializeField] Effect blockedEffect;
    readonly float _rushRate = 0.8f;

    public override void Apply(Skill skill)
    {
        var sourcePos = skill.Owner.transform.position;
        var targetPos = skill.TargetSelectionResult.selectedPosition;

        // Find valid position on NavMesh near target position
        NavMeshHit navHit;
        if (!NavMesh.SamplePosition(targetPos, out navHit, 5f, NavMesh.AllAreas))
        {
            targetPos = sourcePos; // If no valid position found, stay in place
        }
        else
        {
            targetPos = navHit.position;
        }

        var direction = (targetPos - sourcePos).normalized;
        var searchProperRange = skill.TargetSearcher.SearchProperRange;
        var rushDistance = searchProperRange is float ? (float)searchProperRange : ((Vector2)searchProperRange).y;
        var destPos = sourcePos + direction * rushDistance * _rushRate;

        // Check if path is blocked
        NavMeshHit blockedHit;
        if (NavMesh.Raycast(sourcePos, destPos, out blockedHit, NavMesh.AllAreas))
        {
            if (blockedEffect)
            {
                
                Effect effect = blockedEffect.Clone() as Effect;
                effect.Setup(skill, skill.Owner, 1);
                skill.Owner.SkillSystem.Apply(effect);
            }
            destPos = blockedHit.position;
        }

        destPos = destPos.With(z: 0);
        DOTween.To(() => skill.Owner.transform.position, (v) => skill.Owner.transform.position = v, destPos, 0.5f)
            .SetEase(Ease.OutExpo);

        skill.SearchTargets();
        foreach (var target in skill.Targets)
            target.SkillSystem.Apply(skill);
    }

    public override object Clone() => new ChargingRushAction();
}
