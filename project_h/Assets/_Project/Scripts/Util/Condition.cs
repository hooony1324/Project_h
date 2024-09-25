using System;
public abstract class Condition<T> : ICloneable
{
    public abstract bool IsPass(T data);
    public abstract object Clone();
}

