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

        transform.DOPath(new Path(PathType.CatmullRom, pattern.path, 10), pattern.duration, PathMode.Full3D)
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

    Vector3[] pattern_1 = new Vector3[3]
    {
        new(-15, 40, 0),
        new(15, 40, 0),
        new(15, -40, 0),
    };

    Vector3[] pattern_2 = new[] 
    { 
        new Vector3(-20.61734f, -14.54203f, 0f), 
        new Vector3(-21.99646f, 1.571078f, 0f), 
        new Vector3(-12.37478f, 13.52212f, 0f), 
        new Vector3(4.048078f, 16.33737f, 0f), 
        new Vector3(16.3198f, 8.660814f, 0f), 
        new Vector3(21.72606f, -1.295764f, 0f), 
        new Vector3(12.64337f, -11.98045f, 0f), 
        new Vector3(-2.373351f, -12.2533f, 0f), 
        new Vector3(-11.15179f, -6.870964f, 0f), 
        new Vector3(-12.88012f, 2.109316f, 0f), 
        new Vector3(-4.992006f, 8.47467f, 0f), 
        new Vector3(6.216698f, 7.97307f, 0f), 
        new Vector3(11.52579f, -0.493659f, 0f), 
        new Vector3(6.044765f, -10.86063f, 0f), 
        new Vector3(-34.14038f, -41.93391f, 0f) 
    };


}