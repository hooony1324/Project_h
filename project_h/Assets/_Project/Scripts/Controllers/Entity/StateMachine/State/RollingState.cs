public class RollingState : State<Entity>
{
    private PlayerController playerController;

    protected override void Setup()
    {
        playerController = Managers.Game.PlayerController;
    }

    public override void Enter()
    {
        if (playerController)
            playerController.enabled = false;
    }

    public override void Exit()
    {
        if (playerController)
            playerController.enabled = true;
    }
}