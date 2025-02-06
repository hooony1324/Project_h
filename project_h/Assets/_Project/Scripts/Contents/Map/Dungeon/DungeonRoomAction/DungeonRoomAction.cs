


[System.Serializable]
public abstract class DungeonRoomAction
{
    public bool CheckDungeonRoomAction<T>() where T : DungeonRoomAction
        => this.GetType() == typeof(T);
    public abstract void Apply(DungeonRoom dungeonRoom);
}