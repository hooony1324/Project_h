using UnityEngine;


public class UI_Heart : UI_Base
{
    public enum EHeartState
    {
        Empty = 0,          // 완전히 빈 하트
        HalfEmpty = 1,      // 반쪽이 비어있는 반쪽 하트
        Half = 2,           // 반쪽만 채워진 반쪽 하트
        HalfFull = 3,       // 반쪽만 채워진 하트
        Full = 4            // 완전히 채워진 하트
    }

    enum Images
    {
        OnIcon,
        OffIcon,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImages(typeof(Images));

        return true;
    }

    public void SetValue(EHeartState state)
    {
        switch (state)
        {
            case EHeartState.Empty:
                GetImage((int)Images.OnIcon).fillAmount = 0f;
                GetImage((int)Images.OffIcon).fillAmount = 1f;
                break;
            case EHeartState.HalfEmpty:
                GetImage((int)Images.OnIcon).fillAmount = 0f;
                GetImage((int)Images.OffIcon).fillAmount = 0.5f;
                break;
            case EHeartState.Half:
                GetImage((int)Images.OnIcon).fillAmount = 0.5f;
                GetImage((int)Images.OffIcon).fillAmount = 0.5f;
                break;
            case EHeartState.HalfFull:
                GetImage((int)Images.OnIcon).fillAmount = 0.5f;
                GetImage((int)Images.OffIcon).fillAmount = 1f;
                break;
            case EHeartState.Full:
                GetImage((int)Images.OnIcon).fillAmount = 1f;
                GetImage((int)Images.OffIcon).fillAmount = 1f;
                break;
        }
    }
}