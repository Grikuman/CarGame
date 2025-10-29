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

    /// <summary> ���������� </summary>
    public void Initialize(GameObject gameObject)
    {
        _textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    /// <summary> �J�n���� </summary>
    public void OnShow()
    {
        _textMeshPro.color = offColor;
        _titleItemAnimation.FadeOut();
    }
    /// <summary> �X�V���� </summary>
    public void OnUpdate()
    {

    }
    /// <summary> �I������ </summary>
    public void OnHide()
    {
        
    }
}
