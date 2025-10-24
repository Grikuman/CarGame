using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineBoostModule : IVehicleModule, IResettableVehicleModule<MachineBoostModuleData>
{
    public float BoostMultiplier { get; set; }
    public float MaxBoostGauge { get; set; }
    public float GaugeConsumptionRate { get; set; }
    public float GaugeRecoveryRate { get; set; }
    public float BoostCoolDown { get; set; }
    public float CurrentGauge { get; set; }
    public float CoolDownTimer { get; set; }
    public bool IsBoosting { get; set; }

    // エンジンモジュール
    private MachineEngineModule _machineEngineModule;
    // アルティメットモジュール
    private MachineUltimateModule _machineUltimateModule;

    private bool _isActive = true;
    private VehicleController _vehicleController = null;

    /// <summary> アクティブ状態を設定 </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> アクティブ状態を取得 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> 初期化処理 </summary>
    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;

        // 初期のゲージを設定する
        CurrentGauge = MaxBoostGauge;
    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        Debug.Log("Start MachineBoostModule");
        // モジュールデータリセット処理
        _vehicleController.ResetSettings<MachineBoostModuleData>();

        // エンジンモジュールを取得する
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
        // アルティメットモジュールを取得する
        _machineUltimateModule = _vehicleController.Find<MachineUltimateModule>();
    }

    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        Debug.Log("Update MachineBoostModule");

        if (IsBoosting)
        {
            // ゲージを消費
            CurrentGauge -= GaugeConsumptionRate * Time.deltaTime;

            // アルティメットゲージを貯める
            //_machineUltimateModule.AddUltimateGauge();

            // ゲージが無くなった場合ブーストを終了する
            if (CurrentGauge <= 0)
            {
                CurrentGauge = 0;
                EndBoost();
                Debug.Log("ブースト終了");
            }
        }
        else
        {
            // クールダウン中なら回復しない
            if (CoolDownTimer > 0)
            {
                CoolDownTimer -= Time.deltaTime;
            }
            else
            {
                // ゲージを回復
                if (CurrentGauge < MaxBoostGauge)
                {
                    CurrentGauge += GaugeRecoveryRate * Time.deltaTime;
                    CurrentGauge = Mathf.Clamp(CurrentGauge, 0, MaxBoostGauge);
                }
            }
        }
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        Debug.Log("FixedUpdate MachineBoostModule");
    }

    // リセット時の処理
    public void ResetModule(MachineBoostModuleData data)
    {
        Debug.Log("Reset MachineBoostData");

        BoostMultiplier = data.BoostMultiplier;
        MaxBoostGauge = data.MaxBoostGauge;
        GaugeConsumptionRate = data.GaugeConsumptionRate;
        GaugeRecoveryRate = data.GaugeRecoveryRate;
        BoostCoolDown = data.BoostCooldown;
        CurrentGauge = data.CurrentGauge;
        CoolDownTimer = data.CoolDownTimer;
        IsBoosting = data.IsBoosting;

        // 初期のゲージを設定する
        CurrentGauge = MaxBoostGauge;
    }

    /// <summary>
    /// ブースト発動できるか確認する
    /// </summary>
    public void TryActivateBoost()
    {
        // 発動条件
        // ブースト発動中でない　かつ　ゲージが貯まっている　かつクールタイムが終了している場合
        if (!IsBoosting && CurrentGauge > 0 && CoolDownTimer <= 0)
        {
        // アルティメットが発動状態でなければブーストを発動する
        if (!_machineUltimateModule.IsActiveUltimate())
            {
                ActivateBoost();
            }
        }
        ActivateBoost();
    }

    /// <summary>
    /// ブーストを発動する
    /// </summary>
    private void ActivateBoost()
    {
        // マシンエンジンのブースト入力を設定する
        _machineEngineModule.InputBoost = BoostMultiplier;
        // ブースト発動状態
        IsBoosting = true;
        Debug.Log("ブースト発動");
    }

    /// <summary>
    /// ブーストを終了する
    /// </summary>
    private void EndBoost()
    {
        // ブーストの倍率をリセットする
        _machineEngineModule.InputBoost = 1.0f;
        // ブースト発動状態を解除
        IsBoosting = false;
        // ブーストのクールタイムを設定する
        CoolDownTimer = BoostCoolDown;
    }

    /// <summary>
    /// ブーストが有効かどうか
    /// </summary>
    /// <returns>ブーストの発動状態を返す</returns>
    public bool IsActiveBoost()
    {
        return IsBoosting;
    }

    /// <summary>
    /// 現在のゲージ割合を取得する(0～1)
    /// </summary>
    /// <returns>正規化された現在のゲージ割合を返す</returns>
    public float GetBoostGaugeNormalized()
    {
        return CurrentGauge / MaxBoostGauge;
    }
}
