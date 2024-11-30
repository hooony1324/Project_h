using UnityEngine;

[System.Serializable]
public class ImpactAndDestroy : ImpactAction
{


    public override void Apply()
    {
        if (ImpactEffect != null)
            Managers.Object.SpawnEffect(ImpactEffect, Owner.transform.position, Quaternion.identity);

        Managers.Object.DespawnProjectile(Owner);
    }

    public override object Clone() => new ImpactAndDestroy();
}