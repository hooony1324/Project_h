using UnityEngine;

[System.Serializable]
public class CameraShakeAction : CustomAction
{
    public override void Run(object data)
    {
        Managers.Game.Cam.GenerateImpulse(.24f);
    }

    public override object Clone() => new CameraShakeAction();
}