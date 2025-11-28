using UnityEngine;

/// <summary>
/// すべてのアルティメットの共通ベースクラス
/// ・タイマー管理
/// ・発動/終了管理
/// ・MachineEngineModule の参照保持（必要なら使える）
/// </summary>
public abstract class UltimateBase : IUltimate
{
    protected float _ultimateTime;        // 効果時間
    protected float _timer;               // カウントダウン時間
    protected bool _isActive;             // 発動中？
    protected bool _isEnd = false;        // 終了フラグ
    protected MachineEngineModule _engine; // 発動対象のマシン

    /// <summary>
    /// アルティメット発動
    /// </summary>
    public virtual void Activate(MachineEngineModule engine)
    {
        _engine = engine;
        _timer = _ultimateTime;
        _isActive = true;
        _isEnd = false;
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    public virtual void Update()
    {
        if (!_isActive) return;

        _timer -= Time.deltaTime;

        if (_timer <= 0.0f)
        {
            _isEnd = true;
        }
    }

    /// <summary>
    /// アルティメット終了（後処理）
    /// </summary>
    public virtual void End()
    {
        _isActive = false;
        _isEnd = false;
    }

    public bool IsEnd() => _isEnd;
    public bool IsActive() => _isActive;
}
