using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "AbilitySystem/Category")]
public class Category : IdentifiedObject
{
    public override bool Equals(object other)
        => base.Equals(other);

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator==(Category left, string right)
    {
        if (left is null)
            return right is null;
        return left.CodeName == right;
    }

    public static bool operator!=(Category left, string right) => !(left == right);

}