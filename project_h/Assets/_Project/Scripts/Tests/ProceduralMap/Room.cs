using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topLeftDoor;
    [SerializeField] GameObject topRightDoor;
    [SerializeField] GameObject bottomRightDoor;
    [SerializeField] GameObject bottomLeftDoor;

    public int RoomIndex;
    public Vector2Int GridIndex { get; set; }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            topLeftDoor.SetActive(true);
        }

        if (direction == Vector2Int.right)
        {
            topRightDoor.SetActive(true);
        }

        if (direction == Vector2Int.down)
        {
            bottomRightDoor.SetActive(true);
        }

        if (direction == Vector2Int.left)
        {
            bottomLeftDoor.SetActive(true);
        }
    }
}
