using UnityEngine;
using UnityEngine.UI;

public interface INpcInteraction
{
    public void HandleNpcInteraction();
}

public class Npc : Entity
{
    private Sprite npcSprite;
    private UI_NpcInteraction uiNpcInteraction;

    [SerializeReference, SubclassSelector]
    private EntityCondition[] interactCondition;

    [SerializeReference]
    private INpcInteraction npcInteraction;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Npc;
        
        npcSprite = Util.FindChild(gameObject, "NpcSprite").GetComponent<Sprite>();

        uiNpcInteraction = Util.FindChild(gameObject, "UI_NpcInteraction").GetComponent<UI_NpcInteraction>();
        uiNpcInteraction.OnInteraction += OnClickNpc;

        return true;
    }

    public override void SetData(EntityData data)
    {
        base.SetData(data);
        
    }

    private void OnClickNpc()
    {
        //TODO: Interaction check, ex) player Level


    }


}