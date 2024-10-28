using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// PredefinedAssemblyUtil 클래스는 사전 정의된 어셈블리와 상호 작용하는 메서드를 제공합니다.
/// 특정 인터페이스 유형에서 파생된 현재 AppDomain의 모든 타입을 가져올 수 있습니다.
/// 자세한 내용은 <see href="https://docs.unity3d.com/2023.3/Documentation/Manual/ScriptCompileOrderFolders.html">Unity 문서</see>를 참조하십시오.
/// </summary>
public static class PredefinedAssemblyUtil {
    /// <summary>
    /// 탐색을 위한 특정 사전 정의 어셈블리 유형을 정의하는 열거형입니다.
    /// </summary>    
    enum AssemblyType {
        AssemblyCSharp,
        AssemblyCSharpEditor,
        AssemblyCSharpEditorFirstPass,
        AssemblyCSharpFirstPass
    }

    /// <summary>
    /// 어셈블리 이름을 해당 AssemblyType으로 매핑합니다.
    /// </summary>
    /// <param name="assemblyName">어셈블리의 이름입니다.</param>
    /// <returns>일치하는 어셈블리 이름이 없을 경우 null을 반환합니다.</returns>
    static AssemblyType? GetAssemblyType(string assemblyName) {
        return assemblyName switch {
            "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
            "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
            "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
            "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
            _ => null
        };
    }

    /// <summary>
    /// 주어진 어셈블리를 검색하고 특정 인터페이스를 구현하는 타입을 제공된 컬렉션에 추가하는 메서드입니다.
    /// </summary>
    /// <param name="assemblyTypes">어셈블리의 모든 타입을 나타내는 Type 객체 배열입니다.</param>
    /// <param name="interfaceType">확인할 인터페이스 타입입니다.</param>
    /// <param name="results">결과가 추가될 타입 컬렉션입니다.</param>
    static void AddTypesFromAssembly(Type[] assemblyTypes, Type interfaceType, ICollection<Type> results) {
        if (assemblyTypes == null) return;
        for (int i = 0; i < assemblyTypes.Length; i++) {
            Type type = assemblyTypes[i];
            if (type != interfaceType && interfaceType.IsAssignableFrom(type)) {
                results.Add(type);
            }
        }
    }
    
    /// <summary>
    /// 현재 AppDomain의 모든 어셈블리에서 제공된 인터페이스 타입을 구현하는 모든 타입을 가져옵니다.
    /// </summary>
    /// <param name="interfaceType">구현할 모든 타입을 가져올 인터페이스 타입입니다.</param>
    /// <returns>제공된 인터페이스 타입을 구현하는 타입 목록입니다.</returns>    
    public static List<Type> GetTypes(Type interfaceType) {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
        List<Type> types = new List<Type>();
        for (int i = 0; i < assemblies.Length; i++) {
            AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
            if (assemblyType != null) {
                assemblyTypes.Add((AssemblyType) assemblyType, assemblies[i].GetTypes());
            }
        }
        
        assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var assemblyCSharpTypes);
        AddTypesFromAssembly(assemblyCSharpTypes, interfaceType, types);

        assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharpFirstPass, out var assemblyCSharpFirstPassTypes);
        AddTypesFromAssembly(assemblyCSharpFirstPassTypes, interfaceType, types);
        
        return types;
    }
}
