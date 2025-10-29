using UnityEngine;

public interface IButtonAnimationState 
{
    /// <summary> ���������� </summary>
    public void Initialize(GameObject gameObject);

    /// <summary> �J�n���� </summary>
    public void OnShow();
    /// <summary> �X�V���� </summary>
    public void OnUpdate();
    /// <summary> �I������ </summary>
    public void OnHide();
}
