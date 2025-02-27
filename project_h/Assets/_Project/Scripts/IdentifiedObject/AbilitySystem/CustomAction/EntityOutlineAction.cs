using UnityEngine;

[System.Serializable]
public class EntityOutlineAction : CustomAction
{
    Entity entity;
    Material instancedMaterial;

    public override void Start(object data) 
    {
        Effect effect = data as Effect;
        if (effect == null)
            return;

        entity = effect.User as Entity;
        if (entity == null)
            return;

        Renderer renderer = entity.GetComponent<Renderer>();

        if (instancedMaterial == null)
            instancedMaterial = new Material(renderer.sharedMaterial);
        
        instancedMaterial.EnableKeyword("OUTBASE_ON");
        instancedMaterial.SetColor("_OutlineColor", Color.red);
        renderer.sharedMaterial = instancedMaterial;

    }

    public override void Run(object data)
    {

    }

    public override void Release(object data)
    {
        if (entity == null)
            return;

        instancedMaterial.DisableKeyword("OUTBASE_OFF");
        instancedMaterial.SetColor("_OutlineColor", Color.white);
    }

    public override object Clone() => new EntityOutlineAction();
}