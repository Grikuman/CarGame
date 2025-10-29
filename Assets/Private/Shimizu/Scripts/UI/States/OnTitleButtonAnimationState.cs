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

    /// <summary> ���������� </summary>
    public void Initialize(GameObject gameObject)
    {
        _textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    /// <summary> �J�n���� </summary>
    public void OnShow()
    {
        _textMeshPro.color = onColor;

        _titleItemAnimation.FadeIn();
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
