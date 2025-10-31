using UnityEngine;

public abstract class ButtonBase : MonoBehaviour , IButton
{
    /// <summary> ステートを取得する </summary>
    public abstract T GetAnimationState<T>() where T : class, IButtonAnimationState;

    /// <summary> アクティブ状態を設定 </summary>
    public abstract void SetActive(bool value);
    /// <summary> アクティブ状態を取得 </summary>
    public abstract bool GetIsActive();

    /// <summary> ステートを切り替える </summary>
    /// <param name="state">切り替えるステート</param>
    public abstract void ChangeAnimationState(IButtonAnimationState state);

    /// <summary> イベントを発行する </summary>
    public abstract void OnEvent();
}
