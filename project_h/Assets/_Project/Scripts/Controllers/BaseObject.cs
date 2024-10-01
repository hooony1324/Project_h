using System.ComponentModel;
using UnityEngine;

/// <summary>
/// BaseObject : Stat에 의한 상호작용 필요없음
/// InteractionObject : Stat가지고 서로 상호작용(HP깎기) => 나무, 몬스터, 부숴야되는 장애물 등등
/// Creature : 직접적인 전투 관련
/// </summary>

public enum EObjectType
{
    None,
    Hero,
    Monster,
    Camp,
    Env,
    ItemHolder,
    Npc,
    Projectile,
}

public class BaseObject : InitOnce
{
    public EObjectType ObjectType { get; set; }
    public Vector3 Position => transform.position;
    public Vector3 CenterPosition => transform.position + CenterOffset;
    public Vector3 CenterOffset {get; set;} = new Vector3(0, 0.85f, 0);
    public SpriteRenderer Sprite => _sprite;
    private SpriteRenderer _sprite;
    
    bool _lookLeft = true;
    public bool LookLeft
    {
        get { return _lookLeft; }
        set
        {
            _lookLeft = value;
            Flip(!value);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _sprite = GetComponent<SpriteRenderer>();

        return true;
    }
    public void LookAt(BaseObject target)
    {
        if(target == null)
            return;
        Vector2 dir = target.transform.position - transform.position;
        if (dir.x < 0)
            LookLeft = true;
        else if(dir.x > 0)
            LookLeft = false;
    }

    public void LookAt(Vector3 position)
    {
        Vector2 dir = position - transform.position;
        if (dir.x < 0)
            LookLeft = true;
        else if(dir.x > 0)
            LookLeft = false;
    }

    public static Vector3 GetLookAtRotation(Vector3 dir)
    {
        // Mathf.Atan2를 사용해 각도를 계산하고, 라디안에서 도로 변환
        float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        
        // Z축을 기준으로 회전하는 Vector3 값을 리턴
        return new Vector3(0, 0, angle);
    }

    public void Flip(bool flag)
    {
        Sprite.flipX = flag;
    }
  
}