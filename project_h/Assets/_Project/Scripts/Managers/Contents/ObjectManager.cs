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

    public void SpawnFloatingText(Vector2 pos, string message)
    {
        GameObject go = Managers.Resource.Instantiate(nameof(UI_FloatingText), pooling: true);
        UI_FloatingText floatingText = go.GetComponent<UI_FloatingText>();
        floatingText.SetInfo(pos, message);
    }

    public T Spawn<T>(Vector3 spawnPos, string prefabName = "", int templateID = 0) where T : BaseObject
    {
        System.Type type = typeof(T);


        if (type == typeof(Hero))
        {
            GameObject go = Managers.Resource.Instantiate("Hero");
            // go.name = Managers.Data.HeroDic[templateID].DescriptionTextID;
            go.transform.position = spawnPos;
            go.transform.parent = HeroRoot;
            Hero hc = go.GetOrAddComponent<Hero>();
            Heroes.Add(hc);
            Hero = hc;
            return hc as T;
        }
        else if (type == typeof(Monster))
        {
            GameObject go = Managers.Resource.Instantiate("Monster", pooling: true);
            go.transform.position = spawnPos;
            go.transform.parent = MonsterRoot;
            Monster mc = go.GetOrAddComponent<Monster>();
            Monsters.Add(mc);
            return mc as T;
        }

        return null;
    }

    public void Despawn<T>(T baseObject) where T : BaseObject
    {
        System.Type type = typeof(T);

        if (type == typeof(Hero))
        {
            Heroes.Remove(baseObject as Hero);
        }
        else if (type == typeof(Monster))
        {
            Monsters.Remove(baseObject as Monster);
        }

        Managers.Resource.Destroy(baseObject.gameObject);
    }
}