using TMPro;
using UnityEngine;

public class OffResultSceneButtonAnimationState : IButtonAnimationState
{
    private static readonly Color offColor = new Color32(56, 56, 56, 255);
    private TextMeshProUGUI _text;

    public void Initialize(GameObject gameObject)
    {
        _text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void OnShow()
    {
        _text.color = offColor;
    }

    public void OnUpdate() { }

    public void OnHide() { }
}
