using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public HeroCamp HeroCamp { get; set; }
    public Hero Hero { get; private set; }
    public HashSet<Hero> Heroes { get; } = new HashSet<Hero>();
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();
    // public HashSet<Projectile> Projectiles { get; } = new HashSet<Projectile>();
    // public HashSet<Env> Envs { get; } = new HashSet<Env>();
    // public HashSet<Npc> Npcs { get; } = new HashSet<Npc>();
    // public HashSet<ItemHolder> ItemHolders { get; } = new HashSet<ItemHolder>();

    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }
    public Transform HeroRoot { get { return GetRootTransform("@Heroes"); } }
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform ProjectileRoot { get { return GetRootTransform("@Projectiles"); } }
    public Transform EnvRoot { get { return GetRootTransform("@Envs"); } }
    public Transform EffectRoot { get { return GetRootTransform("@Effects"); } }
    public Transform NpcRoot { get { return GetRootTransform("@Npc"); } }
    public Transform ItemHolderRoot { get { return GetRootTransform("@ItemHolders"); } }

    public void Clear()
    {
        //Monsters.Clear();
    }

    // public void ShowDamageFont(Vector2 pos, float damage, Transform parent, EDamageResult result)
    // {
    //     string prefabName = "DamageFont";

    //     GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
    //     DamageFont damageText = go.GetComponent<DamageFont>();
    //     damageText.SetInfo(pos, damage, parent, result);
    // }

    public T Spawn<T>(object position, int templateID = 0, string prefabName = "") where T : BaseObject
    {
        System.Type type = typeof(T);

        Vector3 spawnPos = new Vector3();
        if (position is Vector3)
        {
            spawnPos = (Vector3)position;
        }

        if (type == typeof(HeroCamp))
        {
            GameObject go = Managers.Resource.Instantiate("HeroCamp");
            go.transform.position = spawnPos;
            go.transform.parent = ProjectileRoot;
            go.name = "***CampPoint***";
            HeroCamp= go.GetOrAddComponent<HeroCamp>();
            return HeroCamp as T;
        }

        if (type == typeof(Hero))
        {
            GameObject go = Managers.Resource.Instantiate("Hero");
            // go.name = Managers.Data.HeroDic[templateID].DescriptionTextID;
            go.transform.position = spawnPos;
            go.transform.parent = HeroRoot;
            Hero hc = go.GetOrAddComponent<Hero>();
            Heroes.Add(hc);
            hc.SetInfo(templateID);
            Hero = hc;
            return hc as T;
        }

        if (type == typeof(Monster))
        {
            GameObject go = Managers.Resource.Instantiate("Monster", pooling: true);
            go.transform.position = spawnPos;
            go.transform.parent = MonsterRoot;
            Monster mc = go.GetOrAddComponent<Monster>();
            Monsters.Add(mc);
            mc.SetInfo(templateID);
            return mc as T;
        }

        return null;
    }
}