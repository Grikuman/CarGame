using TMPro;
using UnityEngine;

public class OnTitleSceneButtonAnimationState : IButtonAnimationState
{
    private static readonly Color onColor = Color.white;
    private TextMeshProUGUI _text;

    public void Initialize(GameObject gameObject)
    {
        _text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void OnShow()
    {
        _text.color = onColor;
    }

    public void OnUpdate() { }

    public void OnHide() { }
}
