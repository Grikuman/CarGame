using UnityEngine;

public class SampleModule : IVehicleModule, IResettableVehicleModule<SampleModuleData>
{
    public int data1 { get; set; }
    public int data2{ get; set; }
    public int data3{ get; set; }
    public int data4 { get; set; }

    public float dataf1{ get; set; }
    public float dataf2{ get; set; }
    public float dataf3{ get; set; }
    public float dataf4 { get; set; }

    public Vector3 dataVec1{ get; set; }
    public Vector3 dataVec2{ get; set; }
    public Vector3 dataVec3 { get; set; }


    private bool _isActive = true;
    private VehicleController _vehicleController = null;

    /// <summary> アクティブ状態を設定 </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> アクティブ状態を取得 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> 初期化処理 </summary>
    public void Initialize(VehicleController vehicleController)
    {
        Debug.Log("Initialize Sample Module");

        _vehicleController = vehicleController;
    }
    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        Debug.Log("Update Sample Module");
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        Debug.Log("FixedUpdate Sample Module");
    }


    // リセット時の処理
    public void ResetModule(SampleModuleData data)
    {
        Debug.Log("Reset Sample Module Data");

        data1 = data.Data1;
        data2 = data.Data2;
        data3 = data.Data3;
        data4 = data.Data4;

        dataf1 = data.Dataf1;
        dataf2 = data.Dataf2;
        dataf3 = data.Dataf3;
        dataf4 = data.Dataf4;

        dataVec1 = data.DataVec1;
        dataVec2 = data.DataVec2;
        dataVec3 = data.DataVec3;
    }

}
