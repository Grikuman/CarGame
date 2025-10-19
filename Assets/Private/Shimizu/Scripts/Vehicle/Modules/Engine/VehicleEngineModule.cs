using UnityEngine;

public class VehicleEngineModule : IVehicleModule , IResettableVehicleModule<EngineSettings>
{

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
    }
    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    { 
        Debug.Log("Update Engine Module");
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    { 
        
    }

    // リセット時の処理
    public void ResetModule(EngineSettings settings) 
    {
        Debug.Log("Reset Engine Settings");
    }
}