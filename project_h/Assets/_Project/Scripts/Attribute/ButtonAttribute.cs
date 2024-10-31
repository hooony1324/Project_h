using UnityEngine;

/// <summary>
/// [ButtonAttribute("TestBake")]
/// public bool testBake;
/// public void TestBake()
/// </summary>
public class ButtonAttribute : PropertyAttribute
{
    public string MethodName {get;}
    public ButtonAttribute(string methodName) => MethodName = methodName;
}