
public interface IVehicleModule
{
    /// <summary> アクティブ状態を設定 </summary>
    public void SetActive(bool value);
    /// <summary> アクティブ状態を取得 </summary>
    public bool GetIsActive();
   

    /// <summary> 初期化処理 </summary>
    public void Initialize(VehicleController vehicleController);

    /// <summary> 開始処理 </summary>
    public void Start();

    /// <summary> 更新処理 </summary>
    public void UpdateModule();

    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule();


}
