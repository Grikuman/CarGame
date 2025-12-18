using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : ButtonBase
{
    private bool _isActive;

    private readonly List<IButtonAnimationState> _states = new();
    private IButtonAnimationState _currentState = null;

    /// <summary> ステートを取得する </summary>
    public override T GetAnimationState<T>()
    {
        foreach (var state in _states)
        {
            if (state is T target)
                return target;
        }

        Debug.LogWarning($"State of type {typeof(T).Name} not found in {name}");
        return default;
    }

    private void Awake()
    {
        // ステート作成＆初期化
        AddState(new OnResultSceneButtonAnimationState());
        AddState(new OffResultSceneButtonAnimationState());

        // 初期ステート（非選択）
        _currentState = GetAnimationState<OffResultSceneButtonAnimationState>();
        _currentState.OnShow();
    }

    private void Update()
    {
        // 現在のステートの更新処理
        _currentState?.OnUpdate();
    }

    /// <summary> アクティブ状態を設定 </summary>
    public override void SetActive(bool value)
    {
        _isActive = value;
    }

    /// <summary> アクティブ状態を取得 </summary>
    public override bool GetIsActive()
    {
        return _isActive;
    }

    /// <summary> ステートを切り替える </summary>
    public override void ChangeAnimationState(IButtonAnimationState state)
    {
        if (state == null || state == _currentState)
            return;

        _currentState.OnHide();
        _currentState = state;
        _currentState.OnShow();
    }

    /// <summary> 決定時のイベント </summary>
    public override void OnEvent()
    {
        // シーン遷移
        SceneManager.LoadScene("masamasa");
    }

    /// <summary> ステート追加、初期化 </summary>
    private void AddState(IButtonAnimationState state)
    {
        // このボタン自身を State に渡す
        state.Initialize(gameObject);
        _states.Add(state);
    }
}
