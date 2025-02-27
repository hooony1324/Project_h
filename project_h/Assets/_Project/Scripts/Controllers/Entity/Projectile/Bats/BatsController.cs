using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;

public class BatsController : MonoBehaviour
{
    Bat[] bats;
    BatsPattern[] patterns;

    void Awake()
    {
        patterns = new BatsPattern[2]
        {
            new BatsPattern { duration = 20, path = pattern_1 },
            new BatsPattern { duration = 15, path = pattern_2 }
        };
        
    }

    public void Setup(Projectile parent, Skill skill)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Bat>().Setup(skill);
        }

        // 패턴 랜덤 선택
        int randomIndex = Random.Range(0, patterns.Length);
        randomIndex = 0;
        BatsPattern pattern = patterns[randomIndex];

        // path를 relative하게 변경
        Vector3 startPos = transform.position;
        Vector3[] relativePath = new Vector3[pattern.path.Length];
        for(int i = 0; i < pattern.path.Length; i++)
        {
            relativePath[i] = startPos + pattern.path[i];
        }
        
        transform.DOPath(new Path(PathType.CatmullRom, relativePath, 10), pattern.duration, PathMode.Full3D)
        .OnComplete(() => 
        { 
            Managers.Object.DespawnProjectile(parent);

        }).SetAutoKill(true);
    }

    public struct BatsPattern
    {
        public float duration;
        public Vector3[] path;
    }

    Vector3[] pattern_1 = new[] { new Vector3(-15f,20f,0f), new Vector3(15f,20f,0f), new Vector3(15f,-30f,0f) };
    Vector3[] pattern_2 = new[] { new Vector3(-17f,25f,0f), new Vector3(0f,50f,0f), new Vector3(17f,0f,0f), new Vector3(34f,25f,0f), new Vector3(40f,70f,0f) };

}