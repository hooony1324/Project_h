using static Define;

public class EntityDefaultState : State<Entity>
{
    protected override void Setup() 
    {
        
    }

    public override void Enter() 
    {
        Entity.EnableSearching = true;
    }

    public override void Update() 
    {
        
    }

    public override void Exit() 
    {
        Entity.EnableSearching = false;
    }

    public override bool OnReceiveMessage(int message, object data)
    {
        if ((EntityStateMessage)message != EntityStateMessage.UsingSkill)
            return false;

        var tupleData = ((Skill skill, AnimatorParameter animatorParameter))data;
        Entity.Animator?.SetTrigger(tupleData.Item2.Hash);

        return true;
    }
}