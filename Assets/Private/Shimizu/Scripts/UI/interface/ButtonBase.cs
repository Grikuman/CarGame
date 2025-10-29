using UnityEngine;

public abstract class ButtonBase : MonoBehaviour , IButton
{
    /// <summary> �X�e�[�g���擾���� </summary>
    public abstract T GetAnimationState<T>() where T : class, IButtonAnimationState;

    /// <summary> �A�N�e�B�u��Ԃ�ݒ� </summary>
    public abstract void SetActive(bool value);
    /// <summary> �A�N�e�B�u��Ԃ��擾 </summary>
    public abstract bool GetIsActive();

    /// <summary> �X�e�[�g��؂�ւ��� </summary>
    /// <param name="state">�؂�ւ���X�e�[�g</param>
    public abstract void ChangeAnimationState(IButtonAnimationState state);

    /// <summary> �C�x���g�𔭍s���� </summary>
    public abstract void OnEvent();
}
