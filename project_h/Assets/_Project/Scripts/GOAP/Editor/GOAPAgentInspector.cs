using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoapAgent))]
public class GOAPAgentInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GoapAgent agent = (GoapAgent)target;

        
    }
}

