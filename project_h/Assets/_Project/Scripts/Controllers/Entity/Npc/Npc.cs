using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Define;

public abstract class NpcInteraction
{
    public abstract void HandleNpcInteraction();
}

public class Npc : BaseObject
{
    private UI_NpcInteraction uiNpcInteraction;

    [SerializeReference, SubclassSelector]
    private EntityCondition[] interactConditions;

    [SerializeReference, SubclassSelector]
    private NpcInteraction npcInteraction;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Npc;

        SortingGroup sg = Util.FindChild(gameObject, "NpcSprite").GetOrAddComponent<SortingGroup>();
        sg.sortingOrder = SortingLayers.ENTITY;

        uiNpcInteraction = Util.FindChild(gameObject, "UI_NpcInteraction").GetComponent<UI_NpcInteraction>();
        uiNpcInteraction.OnInteraction += OnClickNpc;

        return true;
    }

    private void OnClickNpc()
    {
        //TODO: Interaction check, ex) player Level
        //interactConditions.All(x => x.IsPass(Entity));

        npcInteraction?.HandleNpcInteraction();
    }


}