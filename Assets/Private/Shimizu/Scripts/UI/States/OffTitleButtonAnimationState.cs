using TMPro;
using UnityEngine;

public class OffTitleButtonAnimationState : IButtonAnimationState
{
    static readonly Color32 offColor = new Color32(56, 56, 56, 255);

    private TextMeshProUGUI _textMeshPro;
    private TitleItemAnimation _titleItemAnimation;

    public OffTitleButtonAnimationState(GameObject ItemObject)
    {
        _titleItemAnimation = ItemObject.GetComponent<TitleItemAnimation>();
    }

    /// <summary> 初期化処理 </summary>
    public void Initialize(GameObject gameObject)
    {
        _textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    /// <summary> 開始処理 </summary>
    public void OnShow()
    {
        _textMeshPro.color = offColor;
        _titleItemAnimation.FadeOut();
    }
    /// <summary> 更新処理 </summary>
    public void OnUpdate()
    {

    }
    /// <summary> 終了処理 </summary>
    public void OnHide()
    {
        
    }
}
