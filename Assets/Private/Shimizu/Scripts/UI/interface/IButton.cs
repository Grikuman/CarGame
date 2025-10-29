
public interface IButton
{
    /// <summary> �X�e�[�g���擾���� </summary>
    public T GetAnimationState<T>() where T : class, IButtonAnimationState;

    /// <summary> �A�N�e�B�u��Ԃ�ݒ� </summary>
    public void SetActive(bool value);
    /// <summary> �A�N�e�B�u��Ԃ��擾 </summary>
    public bool GetIsActive();

    /// <summary> �X�e�[�g��؂�ւ��� </summary>
    /// <param name="state">�؂�ւ���X�e�[�g</param>
    public void ChangeAnimationState(IButtonAnimationState state);

    /// <summary> �C�x���g�𔭍s���� </summary>
    public void OnEvent();

}
