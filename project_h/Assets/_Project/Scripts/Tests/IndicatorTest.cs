using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class IndicatorTest : MonoBehaviour
{
    public GameObject circleIndicatorPrefab;
    public GameObject squareIndicatorPrefab;

    
    async UniTask Start()
    {
        await UniTask.DelayFrame(1);

        var circleIndicator = circleIndicatorPrefab.GetComponent<Indicator>();
        circleIndicator.Setup(new CircleIndicator.CircleArea() { angle = 120f, radius = 6f }, fillAmount: 0.5f);

        var squareIndicator = squareIndicatorPrefab.GetComponent<Indicator>();
        squareIndicator.Setup(new SquareIndicator.SquareArea() { scale = new Vector2(10f, 10f) }, fillAmount: 0.5f);


        
    }

    

    void Update()
    {
        
    }
}
