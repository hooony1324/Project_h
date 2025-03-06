
[System.Serializable]
public abstract class ItemAcquireAction
{
    public abstract void AqcuireAction(Item owner);
    public virtual void Release() {}
    public virtual bool IsSpawnable => true;

    protected Item owner;
    public bool IsActionType<T>() where T : ItemAcquireAction
    {
        if (this.GetType() == typeof(T))
            return true;

        return false;
    }
}
