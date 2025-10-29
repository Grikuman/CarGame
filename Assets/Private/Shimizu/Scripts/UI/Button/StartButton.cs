using System.Collections.Generic;
using UnityEngine;

public class StartButton : ButtonBase
{
    [SerializeField] private GameObject _itemObject;

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
        AddState(new OnTitleButtonAnimationState(_itemObject));
        AddState(new OffTitleButtonAnimationState(_itemObject));

        // 初期ステート設定
        _currentState = GetAnimationState<OffTitleButtonAnimationState>();
        _currentState.OnShow();
    }

    private void Update()
    {
        // 現在のステートの更新処理
        _currentState.OnUpdate();
    }

    /// <summary> アクティブ状態を設定 </summary>
    public override void SetActive(bool value) => _isActive = value;
    /// <summary> アクティブ状態を取得 </summary>
    public override bool GetIsActive() => _isActive;

    /// <summary> ステートを切り替える </summary>
    /// <param name="state">切り替えるステート</param>
    public override void ChangeAnimationState(IButtonAnimationState state)
    {
        // 現在のステートの終了処理
        _currentState.OnHide();

        // ステートの切り替え
        _currentState = state;

        // 次のステートの開始処理
        _currentState.OnShow();
    }

    /// <summary> イベントを発行する </summary>
    public override void OnEvent()
    {

    }


    /// <summary> ステート追加、初期化 </summary>
    private void AddState(IButtonAnimationState state)
    {
        state.Initialize(gameObject);
        _states.Add(state);
    }
}
