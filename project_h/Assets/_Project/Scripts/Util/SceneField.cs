using UnityEngine;

/// <summary>
/// use: [SerializeField] private SceneField scene1;
/// </summary>
[System.Serializable]
public class SceneField
{
    [SerializeField]
    private Object sceneAsset;

    [SerializeField]
    private string sceneName;
    public string SceneName => sceneName;

    // makes it work with the existing Unity methods (LoadLevel/LoadScene)
    public static implicit operator string(SceneField sceneField) => sceneField.sceneName;
}