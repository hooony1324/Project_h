using Unity.Behavior;

public class Monster : Entity
{
    MonsterData monsterData;
    UI_WorldText infoText;
    BehaviorGraphAgent bga;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        infoText = Util.FindChild<UI_WorldText>(gameObject);
        layerMask = Util.GetLayerMask("Monster");
        enemyLayerMask = Util.GetLayerMask("Hero");

        bga = GetComponent<BehaviorGraphAgent>();

        return true;
    }
    
    public override void SetData(EntityData data)
    {
        base.SetData(data);

        bga.Restart();

        onDead -= HandleOnDead;
        onDead += HandleOnDead;

        monsterData = data as MonsterData;
    }

    public override void TakeDamage(Entity instigator, object causer, float damage)
    {
        base.TakeDamage(instigator, causer, damage);

        Target = Managers.Hero.MainHero;
    }

    private void HandleOnDead(Entity entity)
    {
        Target = null;
        Movement.TraceTarget = null;

        _ = Managers.Dungeon.DropItem(monsterData.DropGroupID, CenterPosition);
        
        Invoke("Despawn", 3.0f);
    }

    private void Despawn()
    {

        Managers.Object.Despawn(this);
    }

    void Update()
    {
        if (StateMachine != null)
        {
            infoText.SetInfo(StateMachine.GetCurrentState().ToString());
        }
    }
}