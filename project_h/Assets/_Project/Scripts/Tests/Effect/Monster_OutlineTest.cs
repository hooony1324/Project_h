
using Unity.VisualScripting;
using UnityEngine;

public class Monster_OutlineTest : MonoBehaviour 
{

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        
        Material mat = new Material(renderer.sharedMaterial);
        mat.EnableKeyword("OUTBASE_ON");
        mat.SetColor("_OutlineColor", Color.red);

        renderer.sharedMaterial = mat;
    }
}
