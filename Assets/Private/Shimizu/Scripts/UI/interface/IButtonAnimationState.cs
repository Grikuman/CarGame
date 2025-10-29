using UnityEngine;

public interface IButtonAnimationState 
{
    /// <summary> 初期化処理 </summary>
    public void Initialize(GameObject gameObject);

    /// <summary> 開始処理 </summary>
    public void OnShow();
    /// <summary> 更新処理 </summary>
    public void OnUpdate();
    /// <summary> 終了処理 </summary>
    public void OnHide();
}
