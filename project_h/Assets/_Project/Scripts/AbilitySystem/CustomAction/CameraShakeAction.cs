using UnityEngine;

[System.Serializable]
public class CameraShakeAction : CustomAction
{
    public override void Run(object data)
    {
        // cinemachine impulse
    }

    public override object Clone() => new CameraShakeAction();
}