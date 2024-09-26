using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public abstract class TargetSearchAction : ICloneable
{
    public abstract object Clone();
}