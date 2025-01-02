using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProjectileAction : SkillAction
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private string spawnPointSocketName;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private bool useObjectPool = true;

    [SerializeReference, SubclassSelector]
    private ImpactAction impactAction;

    [SerializeReference, SubclassSelector]
    private ProjectileMotion projectileMotion;

    [SerializeField]
    [Tooltip("기본 Direction을 기준으로 상대적인 발사 각도를 설정, Info가 한개도 없다면 기본 Direction으로 발사")]
    private ShootInfo[] shootInfos;
    
    public override void Apply(Skill skill)
    {
        var socket = skill.Owner.GetFireSocket();
        Vector3 spawnPosition = socket ? socket.position : skill.Owner.CenterPosition;

        // Projectile을 + 누르면 계속 추가
        // Projectile마다 방향 설정 가능


        // TargetSearcher > Selection Action : Select By Moving Direction이면
        Vector3 direction = skill.Owner.Movement.MovedDirection;

        // TargetSearcher > Selection Action : Select Auto By Targetting Range이면
        var targetSelectionResult = skill.TargetSelectionResult;
        if (targetSelectionResult.resultMessage == SearchResultMessage.FindTarget)
            direction = targetSelectionResult.selectedTarget.transform.position - spawnPosition;
        else if (targetSelectionResult.resultMessage == SearchResultMessage.FindPosition)
            direction = targetSelectionResult.selectedPosition;


        direction = direction.With(z:0).normalized;

        if (shootInfos.Length == 0)
        {
            var projectile = Managers.Object.SpawnProjectile(projectilePrefab, spawnPosition, Quaternion.identity, useObjectPool);
            projectile.transform.position = spawnPosition;
            projectile.GetComponent<Projectile>().Setup(skill.Owner, speed, direction, skill, lifetime, impactAction, projectileMotion);
        }
        else
        {
            foreach (var shootInfo in shootInfos)
            {
                var projectile = Managers.Object.SpawnProjectile(projectilePrefab, spawnPosition, Quaternion.identity, useObjectPool);
                projectile.transform.position = spawnPosition;

                // direction을 angle만큼 회전
                var angle = shootInfo.angle;
                Vector3 shootDir = Quaternion.Euler(0, 0, angle) * direction;
                projectile.GetComponent<Projectile>().Setup(skill.Owner, speed, shootDir, skill, lifetime, impactAction, projectileMotion);
            }
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
            projectileMotion = projectileMotion,
            lifetime = lifetime,
        };
    }

    [Serializable]
    public class ShootInfo
    {
        public float angle;
    }
}
