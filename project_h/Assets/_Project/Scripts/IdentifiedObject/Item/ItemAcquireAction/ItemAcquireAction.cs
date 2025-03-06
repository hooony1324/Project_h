
[System.Serializable]
public abstract class ItemAcquireAction
{
    public abstract void AqcuireAction(Item owner);
    public virtual void Release() {}
    public virtual bool IsSpawnable => true;

    protected Item owner;
    public bool IsActionType<T>() where T : ItemAcquireAction
    {
        return this.GetType() == typeof(T);
    }

    public bool IsSubclassOf<T>() where T : ItemAcquireAction
    {
        return this.GetType().IsSubclassOf(typeof(T));
    }
}
