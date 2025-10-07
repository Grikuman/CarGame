// ============================================
// 
// ファイル名: SteeringController.cs
// 概要: ロジクール系ステアリングコントローラーの抽象化クラス（シングルトン）
// 
// 製作者 : 清水駿希
// 
// ============================================
using System;
using Logitech;
using UnityEngine;

public class SteeringController
{
    public enum  ButtonID : int
    {
        A       = 0,
        X       = 1,
        B       = 2,
        Y       = 3,
        R_SHIFT = 4,
        L_SHIFT = 5,
        R2      = 6,
        L2      = 7,
        SHARE   = 8,
        OPTIONS = 9,
        R3      = 10,
        L3      = 11,
    };

    public enum POVDirection : int
    {
        CENTER     = -1,
        UP         = 0,
        UP_RIGHT   = 4500,
        RIGHT      = 9000,
        DOWN_RIGHT = 13500,
        DOWN       = 18000,
        DOWN_LEFT  = 22500,
        LEFT       = 27000,
        UP_LEFT    = 31500,
    }

    // 入力値の構造体
    private LogitechGSDK.DIJOYSTATE2ENGINES _rec;

    // 前フレームの入力値
    private byte[] _buttons;

    // 前フレームのPOV値
    private int _prevPOV = -1;
    // 今フレームのPOV値
    private int _currentPOV = -1;

    // 自動センタリング（自動でゼロに戻る）の有効/無効フラグ
    public bool _isAutoCenteringActive { get; set; } = true;



    // サチュレーション率（FFBの最大入力に到達するまでの割合）
    private int _saturationPercentage = 10000;
    // コエフィシェント率（FFBの強さ倍率）
    public int _coefficientPercentage = 5000;
    // ステアリングの感度
    private float _steeringSensitivity;
    // アクセル感度
    private float _AcceleratorSensitivity;
    // ブレーキ感度
    private float _BrakeSensitivity;



    // シングルトンインスタンス
    private static SteeringController _instance;
    public static SteeringController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SteeringController();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    /// <summary>
    /// ステアリングコントローラーが接続されているか
    /// </summary>
    public bool GetState() => LogitechGSDK.LogiIsConnected(0);

    /// <summary>
    /// ロジクール系ステアリングコントローラーのサチュレーション率<br/>
    /// (FFBの最大入力に到達するまでの割合)<br/>
    /// <c>0 ～ 10000</c> の範囲で設定してください。
    /// </summary>
    public int saturationPercentage 
    {
        get => _saturationPercentage;
        set
        {
            if (value > 10000)
                throw new ArgumentOutOfRangeException(nameof(saturationPercentage), "最大値は10000です。");
            _saturationPercentage = value;
        }
    }
    /// <summary>
    /// ロジクール系ステアリングコントローラーのコエフィシェント率<br/>
    /// (FFBの強さ倍率)<br/>
    /// <c>0 ～ 5000</c> の範囲で設定してください。
    /// </summary>
    public int coefficientPercentage
    {
        get => _coefficientPercentage;
        set
        {
            if (value > 5000)
                throw new ArgumentOutOfRangeException(nameof(coefficientPercentage), "最大値は5000です。");
            _coefficientPercentage = value;
        }
    }



    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        // デバイスの初期化処理
        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
        // 入力の状態を更新
        _rec = LogitechGSDK.LogiGetStateUnity(0);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public void Update()
    {
        // コントローラーが接続されている場合SDKを更新する
        if(LogitechGSDK.LogiIsConnected(0))
        {
            // 更新前の入力値を取得する
            _buttons = _rec.rgbButtons;
            _prevPOV = _currentPOV;

            // センタリング処理
            // 有効化
            if (_isAutoCenteringActive) LogitechGSDK.LogiPlaySpringForce(0, 0, 10000, 5000);
            // 解除
            else LogitechGSDK.LogiStopSpringForce(0);


            // ロジクールSDKを更新する
            if (LogitechGSDK.LogiUpdate())
            {
                // 入力の状態を更新
                _rec = LogitechGSDK.LogiGetStateUnity(0);
                _currentPOV = (int)_rec.rgdwPOV[0];

            }
        }
    }

    /// <summary>
    /// アプリケーション終了処理
    /// </summary>
    public void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }



    /// <summary>
    /// ボタンが押されている状態
    /// </summary>
    public bool GetButtonIsPressed(ButtonID id)
    {
        // 一般的な「今の状態が押してる」判定
        return _rec.rgbButtons[(int)id] == 128;
    }

    /// <summary>
    /// ボタンが押された瞬間
    /// </summary>
    public bool GetButtonWasPressedThisFrame(ButtonID id)
    {
        // 前フレームで押されていたら
        if (_buttons[(int)id] == 128)
        {
            return false;
        }
        // 押されていないならTrueを返す
        return _rec.rgbButtons[(int)id] == 128;
    }

    /// <summary>
    /// ボタンが離された瞬間
    /// </summary>
    public bool GetButtonWasReleasedThisFrame(ButtonID id)
    {
        // 前フレームで押されていたら
        if (_buttons[(int)id] == 128)
        {
            // 現在のフレーム離されていたら
            return _rec.rgbButtons[(int)id] != 128;
        }

        return false;
    }


    /// <summary>
    /// 現在指定方向が押されているか
    /// </summary>
    public bool GetPOVIsPressed(POVDirection id)
    {
        return _currentPOV == (int)id;
    }

    /// <summary>
    /// 指定方向が押された瞬間
    /// </summary>
    public bool GetPOVWasPressedThisFrame(POVDirection id)
    {
        return _prevPOV != (int)id && _currentPOV == (int)id;
    }

    /// <summary>
    /// 指定方向が離された瞬間
    /// </summary>
    public bool GetPOVWasReleasedThisFrame(POVDirection id)
    {
        return _prevPOV == (int)id && _currentPOV != (int)id;
    }

    /// <summary>
    /// ステアリングの座標を取得する
    /// </summary>
    /// <returns>ステアリングの正規化数値</returns>
    public float GetSteeringPosition()
    {
        // ロジクールデバイスが接続されているかどうか確認
        if (LogitechGSDK.LogiIsConnected(0))
        {
            // ステアリングの値（ -32768 ～ 32767）を-1 ～ 1に正規化
            float normalized = Mathf.Clamp(_rec.lX / 32767f, -1f, 1f);

            // 小数点第二位まで切り捨て
            return Mathf.Floor(normalized * 100f) * 0.01f;
        }

        return 0f;
    }

    /// <summary>
    /// アクセルの座標を取得する
    /// </summary>
    /// <returns>アクセルの正規化数値</returns>
    public float GetAcceleratorPosition()
    {
        // ロジクールデバイスが接続されているかどうか確認
        if (LogitechGSDK.LogiIsConnected(0))
        {
            // アクセルの値（ -32768 ～ 32767）を0 ～ 1に正規化
            float normalized = Mathf.Clamp01((32767f - _rec.lY) / 65535f);

            // 小数点第二位まで切り捨て
            return Mathf.Floor(normalized * 100f) * 0.01f;
        }

        return 0f;
    }

    /// <summary>
    /// ブレーキの座標を取得する
    /// </summary>
    /// <returns>ブレーキの正規化数値</returns>
    public float GetBrakePosition()
    {
        // ロジクールデバイスが接続されているかどうか確認
        if (LogitechGSDK.LogiIsConnected(0))
        {
            // アクセルの値（ -32768 ～ 32767）を0 ～ 1に正規化
            float normalized = Mathf.Clamp01((32767f - _rec.lRz) / 65535f);

            // 小数点第二位まで切り捨て
            return Mathf.Floor(normalized * 100f) * 0.01f;
        }

        return 0f;
    }

}
