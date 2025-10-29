using TMPro;
using UnityEngine;

public class OnTitleButtonAnimationState : IButtonAnimationState
{
    static readonly Color32 onColor = Color.white;


    private TextMeshProUGUI _textMeshPro;
    private TitleItemAnimation _titleItemAnimation;

    public OnTitleButtonAnimationState(GameObject ItemObject)
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
        _textMeshPro.color = onColor;

        _titleItemAnimation.FadeIn();
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
