using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public class ObjectManager
{
    public HeroCamp HeroCamp { get; set; }
    public Hero Hero { get; private set; }
    public HashSet<Hero> Heroes { get; } = new HashSet<Hero>();
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();
    public HashSet<DungeonDoor> Doors { get; } = new HashSet<DungeonDoor>();
    public HashSet<Projectile> Projectiles { get; } = new HashSet<Projectile>();
    public HashSet<GameObject> Effects { get; } = new HashSet<GameObject>();
    // public HashSet<Env> Envs { get; } = new HashSet<Env>();
    // public HashSet<Npc> Npcs { get; } = new HashSet<Npc>();
    public HashSet<ItemHolder> ItemHolders { get; } = new HashSet<ItemHolder>();

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
    public Transform DoorRoot { get { return GetRootTransform("@Doors"); } }

    public void Clear()
    {
        Monsters.Clear();
        Projectiles.Clear();
        Doors.Clear();
    }

    public void SpawnFloatingText(Vector2 pos, string message, EFloatingTextType type = EFloatingTextType.Damage)
    {
        GameObject go = Managers.Resource.Instantiate(nameof(UI_FloatingText), pooling: true);
        UI_FloatingText floatingText = go.GetComponent<UI_FloatingText>();
        floatingText.SetInfo(message, pos, type);
    }

    public T Spawn<T>(Vector3 spawnPos, string prefabName = "", string dataName = "") where T : BaseObject
    {
        System.Type type = typeof(T);

        if (type == typeof(Hero))
        {
            GameObject go = Managers.Resource.Instantiate("Hero");
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
        else if (type == typeof(DungeonDoor))
        {
            GameObject go = Managers.Resource.Instantiate(nameof(DungeonDoor), pooling: true);
            go.transform.position = spawnPos;
            go.transform.parent = DoorRoot;
            DungeonDoor dc = go.GetOrAddComponent<DungeonDoor>();
            Doors.Add(dc);
            return dc as T;
        }
        else if (type == typeof(ItemHolder))
        {
            GameObject go = Managers.Resource.Instantiate(nameof(ItemHolder), pooling: true);
            go.transform.position = spawnPos;
            ItemHolder itemHolder = go.GetOrAddComponent<ItemHolder>();
            ItemHolders.Add(itemHolder);
            return itemHolder as T;
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
        else if (type == typeof(DungeonDoor))
        {
            Doors.Remove(baseObject as DungeonDoor);
        }
        else if (type == typeof(ItemHolder))
        {
            ItemHolders.Remove(baseObject as ItemHolder);
        }

        Managers.Resource.Destroy(baseObject.gameObject);
    }

    public Projectile SpawnProjectile(GameObject prefab, Vector3 position, Quaternion rotation, bool bUsePool = true)
    {
        GameObject go = Managers.Resource.Instantiate(prefab.name, pooling: bUsePool);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.parent = ProjectileRoot;
        Projectile projectile = go.GetOrAddComponent<Projectile>();
        Projectiles.Add(projectile);
        return go.GetOrAddComponent<Projectile>();
    }

    public void DespawnProjectile(Projectile projectile)
    {
        Projectiles.Remove(projectile);
        Managers.Resource.Destroy(projectile.gameObject);
    }

    public GameObject SpawnEffect(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject go = Managers.Resource.Instantiate(prefab.name, EffectRoot, pooling: true);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.parent = EffectRoot;
        go.AddComponent<SortingGroup>().sortingOrder = SortingLayers.SKILL_EFFECT;
        Effects.Add(go);
        return go;
    }

    public GameObject SpawnEffect(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject go = Managers.Resource.Instantiate(prefabName, EffectRoot, pooling: true);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.parent = EffectRoot;
        go.AddComponent<SortingGroup>().sortingOrder = SortingLayers.SKILL_EFFECT;
        Effects.Add(go);
        return go;
    }

    public void DespawnEffect(GameObject effect)
    {
        Effects.Remove(effect);
        Managers.Resource.Destroy(effect);
    }

    public void ClearSpawnedObjects()
    {
        foreach (Monster monster in Monsters)
        {
            Managers.Resource.Destroy(monster.gameObject);
        }
        Monsters.Clear();
    }
}