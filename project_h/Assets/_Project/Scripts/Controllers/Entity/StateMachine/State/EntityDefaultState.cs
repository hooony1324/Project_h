using static Define;

public class EntityDefaultState : State<Entity>
{

    public override void Enter() 
    {
        Entity.Movement.StartTrace();
    }

    public override void Exit() 
    {
        Entity.Movement.StopTrace();
    }

    public override void Update()
    {
        if (!Entity.Target)
            return;
            
        Entity.Movement.FindPathAndMoveToCellPos(Entity.Target.position, HERO_DEFAULT_MOVE_DEPTH);
    }

    // 트리거스킬 발동 시 사용
    // - 이동스킬에 용이
    public override bool OnReceiveMessage(int message, object data)
    {
        if ((EntityStateMessage)message != EntityStateMessage.UsingSkill)
            return false;

        var tupleData = ((Skill skill, AnimatorParameter animatorParameter))data;
        Entity.Animator?.SetTrigger(tupleData.Item2.Hash);

        return true;
    }
}