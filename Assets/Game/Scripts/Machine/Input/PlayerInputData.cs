using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Player Input Module Data")]
public class PlayerInputModuleData : VehicleModuleFactoryBase
{
    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var playerInputModule = new PlayerInputModule();

        // 初期設定

        // 初期化処理
        playerInputModule.Initialize(vehicleController);

        return playerInputModule;
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<PlayerInputModuleData> playerInputModule)
        {
            playerInputModule.ResetModule(this);
        }
    }
}
