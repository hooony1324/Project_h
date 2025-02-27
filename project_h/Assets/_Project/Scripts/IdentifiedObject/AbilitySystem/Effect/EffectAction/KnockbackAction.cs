using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class KnockbackAction : EffectAction
{
    [SerializeField] private Category removeTargetCategory;
    
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;    // 넉백 힘
    [SerializeField] private float knockbackDuration = 0.5f;    // 넉백 지속시간
    [SerializeField] private Ease knockbackEase = Ease.OutQuad;    // 넉백 이징

    public override void Start(Effect effect, Entity user, Entity target, int level, float scale)
    {
        if (target == null || target.IsDead)
            return;

        

        // 넉백 방향 계산 (사용자로부터 대상을 향하는 방향)
        Vector2 direction = (target.transform.position - user.transform.position).normalized;
        
        // 시작 위치 저장
        Vector2 startPos = target.transform.position;
        // 목표 위치 계산 (힘 * 스케일 만큼 이동)
        Vector2 endPos = startPos + direction * (knockbackForce * scale);

        // DOTween을 사용하여 넉백 애니메이션 실행
        target.transform.DOMove(endPos, knockbackDuration)
            .SetEase(knockbackEase)
            .OnComplete(() => {
                // 넉백 완료 후 추가 처리가 필요한 경우
                
            });
    }

    public override bool Apply(Effect effect, Entity user, Entity target, int level, int stack, float scale)
    {
        if (removeTargetCategory != null)
            target.SkillSystem.RemoveEffectAll(x => x != effect && x.HasCategory(removeTargetCategory));

        return true;
    }

    public override object Clone()
    {
        return new KnockbackAction()
        {
            removeTargetCategory = removeTargetCategory,
            knockbackForce = knockbackForce,
            knockbackDuration = knockbackDuration,
            knockbackEase = knockbackEase,
        };
    }
}