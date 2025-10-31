
public interface IButton
{
    /// <summary> ステートを取得する </summary>
    public T GetAnimationState<T>() where T : class, IButtonAnimationState;

    /// <summary> アクティブ状態を設定 </summary>
    public void SetActive(bool value);
    /// <summary> アクティブ状態を取得 </summary>
    public bool GetIsActive();

    /// <summary> ステートを切り替える </summary>
    /// <param name="state">切り替えるステート</param>
    public void ChangeAnimationState(IButtonAnimationState state);

    /// <summary> イベントを発行する </summary>
    public void OnEvent();

}
