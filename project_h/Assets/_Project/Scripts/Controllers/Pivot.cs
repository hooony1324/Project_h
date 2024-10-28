using Unity.VisualScripting;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    private Transform pivot;
    public Transform Destination { get; protected set; }
    
    private void Awake()
    {
        pivot = this.transform;
        Destination = Util.FindChild<Transform>(pivot.gameObject, "Destination", true);

        Destination.GetComponent<SpriteRenderer>().sortingOrder = Define.SortingLayers.JOYSTICK;
    }

    public void SetRadius(float radius = 3f)
    {
        Destination.localPosition = new Vector3(0, radius, 0);
    }

    public void SetAngle(float angle)
    {
        pivot.eulerAngles = new Vector3(0, 0 , angle);
    }
}