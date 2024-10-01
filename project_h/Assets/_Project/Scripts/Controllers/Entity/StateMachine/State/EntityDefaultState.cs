using static Define;

public class EntityDefaultState : State<Entity>
{

    public override void Enter() 
    {

    }

    public override void Exit() 
    {

    }

    public override void Update()
    {
        

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