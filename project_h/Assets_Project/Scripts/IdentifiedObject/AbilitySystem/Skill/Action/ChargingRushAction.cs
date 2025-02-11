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
    NavMeshHit hit;
    if (NavMesh.Raycast(sourcePos, destPos, out hit, NavMesh.AllAreas))
    {
        Debug.Log("벽에 부딪힘!");
        destPos = hit.position;
    }

    destPos = destPos.With(z: 0);
    DOTween.To(() => skill.Owner.transform.position, (v) => skill.Owner.transform.position = v, destPos, 0.5f)
        .SetEase(Ease.OutExpo);

    skill.SearchTargets();
    foreach (var target in skill.Targets)
        target.SkillSystem.Apply(skill);
}
