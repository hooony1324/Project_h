using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

public enum AnimatorParameterType
{
    Bool,
    Trigger
}

[System.Serializable]
[InlineProperty]
public struct AnimatorParameter
{
    [HorizontalGroup("Split", width: 70), HideLabel]
    public AnimatorParameterType type;
    [HorizontalGroup("Split"), HideLabel]
    public string name;

    private int hash;

    public bool IsValid => !string.IsNullOrEmpty(name);
    public int Hash
    {
        get
        {
            if (hash == 0 && IsValid)
                hash = Animator.StringToHash(name);
            return hash;
        }
    }
}
