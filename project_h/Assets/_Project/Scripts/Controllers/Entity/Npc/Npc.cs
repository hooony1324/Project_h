using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

using static Define;

public abstract class NpcInteraction
{
    public abstract void Setup(Npc npc);
    public abstract void HandleNpcInteraction();
}

public class Npc : BaseObject
{
    private TextMeshPro _npcNameText;
    private UI_NpcInteraction _uiNpcInteraction;

    [HelpBox("isClickInteraction(true) : NPC클릭으로 Interaction\nisClickIn teraction(false) : NPC영역에 충돌하여 Interaction")]
    [SerializeField]
    private bool _isClickInteraction = true;


    [Space(20)]
    [HelpBox("Interaction에 제한 설정\n - ex) 레벨 10이 안되었으니 던전 입장 불가 \n - ex) 퀘스트 안 깨서 대화 못함")]
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

        _uiNpcInteraction = Util.FindChild(gameObject, "UI_NpcInteraction").GetComponent<UI_NpcInteraction>();
        
        _npcNameText = Util.FindChild(gameObject, "NpcNameText").GetComponent<TextMeshPro>();
        _npcNameText.text = "TESTNAME";
        return true;
    }

    private void Start()
    {
        _uiNpcInteraction.SetInfo(OnNpcInteracted, _isClickInteraction);
        npcInteraction.Setup(this);
    }

    private void OnNpcInteracted()
    {
        //TODO: Interaction check, ex) player Level
        //interactConditions.All(x => x.IsPass(Entity));

        npcInteraction?.HandleNpcInteraction();
    }


}