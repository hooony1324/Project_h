using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProjectileAction : SkillAction
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private string spawnPointSocketName;
    [SerializeField]
    private float speed;

    [SerializeReference, SubclassSelector]
    private ImpactAction impactAction;

    [SerializeReference, SubclassSelector]
    private ProjectileMotion projectileMotion;

    [SerializeField]
    private ShootInfo[] shootInfos;
    
    public override void Apply(Skill skill)
    {
        var socket = skill.Owner.GetFireSocket();

        // Projectile을 + 누르면 계속 추가
        // Projectile마다 방향 설정 가능


        // TargetSearcher > Selection Action : Select By Moving Direction이면
        Vector3 direction = skill.Owner.Movement.MovedDirection;
        
        // TargetSearcher > Selection Action : Select Auto By Targetting Range이면
        var targetSelectionResult = skill.TargetSelectionResult;
        if (targetSelectionResult.resultMessage == SearchResultMessage.FindTarget)
            direction = targetSelectionResult.selectedTarget.transform.position - socket.position;
        
        // Use한 뒤로 IsDefaultAttack방향 설정 부분 체크
        // 자동 타게팅 발사 -> 방향 lOOK
        // 아니면 MovedDir 그대로 LOOK

        foreach (var shootInfo in shootInfos)
        {
            var projectile = Managers.Object.SpawnProjectile(projectilePrefab, socket.position, socket.rotation);
            projectile.transform.position = socket.position;

            // direction을 angle만큼 회전
            var angle = shootInfo.angle;
            Vector3 shootDir = Quaternion.Euler(0, 0, angle) * direction;
            _ = projectile.GetComponent<Projectile>().Setup(skill.Owner, speed, shootDir, skill, impactAction, projectileMotion);
        }
        
    }

    public override object Clone()
    {
        return new SpawnProjectileAction()
        {
            projectilePrefab = projectilePrefab,
            spawnPointSocketName = spawnPointSocketName,
            speed = speed,
            shootInfos = shootInfos,
            impactAction = impactAction,
            projectileMotion = projectileMotion
        };
    }

    [Serializable]
    public class ShootInfo
    {
        public float angle;
    }
}
