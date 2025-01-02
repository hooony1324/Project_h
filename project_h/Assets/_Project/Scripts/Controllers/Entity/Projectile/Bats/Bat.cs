using UnityEngine;
using DG.Tweening;
using static Define;

public class Bat : MonoBehaviour
{
    private Skill skill;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();       
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = SortingLayers.PROJECTILE;
        // 랜덤한 시작 시간 설정 (0~1초 사이)
        animator.Play(0, -1, Random.Range(0f, 1f));
    }

    public void Setup(Skill skill)
    {
        this.skill = skill;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        
        SkillSystem targetSkillSystem = other.gameObject.GetComponent<SkillSystem>();
        targetSkillSystem.Apply(skill);
    }
}