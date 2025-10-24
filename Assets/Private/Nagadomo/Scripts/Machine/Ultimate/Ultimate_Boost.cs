using UnityEngine;

public class Ultimate_Boost : IUltimate
{
    private float _ultimateTime = 3.0f;   // アルティメットの効果時間
    private float _boostMultiplier = 2.5f;// ブーストの倍率
    private float _timer;                 // タイマー
    private bool _isActive;               // 発動状態の管理
    private bool _isEnd = false;          // 効果時間終了通知
    private MachineEngineModule _machineEngineModule;

    /// <summary>
    /// 発動する
    /// </summary>
    /// <param name="machineEngineController">マシンエンジンコントローラー</param>
    public void Activate(MachineEngineModule machineEngineModule)
    {
        // マシンエンジンコントローラーを設定する
        _machineEngineModule = machineEngineModule;
        // ブーストの倍率を設定する
        _machineEngineModule.InputBoost = _boostMultiplier;
        // アルティメットの効果時間を設定する
        _timer = _ultimateTime;
        // アルティメットを発動状態にする
        _isActive = true;
    }

    /// <summary>
    /// 更新する
    /// </summary>
    public void Update()
    {
        if (!_isActive) return;

        _timer -= Time.deltaTime;
        // アルティメット時間が終了したら
        if (_timer <= 0.0f)
        {
            // 終了させる
            _isEnd = true;
        }
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void End()
    {
        // ブーストの倍率をリセットする
        _machineEngineModule.InputBoost = 1.0f;
        // 発動状態を解除する
        _isActive = false;
        // 終了状態を解除する
        _isEnd = false;
    }

    /// <summary>
    /// アルティメットの効果が終了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsEnd() { return _isEnd; }

    /// <summary>
    /// アルティメットが発動中かどうか
    /// </summary>
    /// <returns>発動状態</returns>
    public bool IsActive() { return _isActive; }
}
