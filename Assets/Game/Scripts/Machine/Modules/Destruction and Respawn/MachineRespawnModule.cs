using UnityEngine;
using System.Collections;

public class MachineRespawnModule : IVehicleModule, IResettableVehicleModule<MachineRespawnModuleData>
{
    public float RespawnDelay { get; set; }
    public GameObject ExplosionPrefab { get; set; }
    public float ExplosionScale { get; set; }

    private bool _isActive = true;
    private bool _isDead = false;

    private VehicleController _vehicleController;
    private Rigidbody _rb;

    // リスポーン時に無効化するモジュール
    private MachineEngineModule _machineEngineModule;
    private MachineSteeringModule _machineSteeringModule;
    private PlayerInputModule _playerInputModule;

    public void SetActive(bool value) => _isActive = value;
    public bool GetIsActive() => _isActive;

    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;
        _rb = vehicleController.GetComponent<Rigidbody>();
    }

    public void Start()
    {
        // 設定リセット
        _vehicleController.ResetSettings<MachineRespawnModuleData>();

        // リスポーン時に無効化するモジュールを取得する
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
        _machineSteeringModule = _vehicleController.Find<MachineSteeringModule>();
        _playerInputModule = _vehicleController.Find<PlayerInputModule>();
    }

    public void UpdateModule() { }
    public void FixedUpdateModule() { }

    public void ResetModule(MachineRespawnModuleData data)
    {
        RespawnDelay = data.RespawnDelay;
        ExplosionPrefab = data.ExplosionPrefab;
        ExplosionScale = data.ExplosionScale;
        _isDead = false;
    }

    /// <summary>
    /// MachineDestructionModuleから呼ばれる
    /// </summary>
    public void Kill()
    {
        if (_isDead) return;
        _isDead = true;

        // 操作を止める
        _machineEngineModule?.SetActive(false);
        _machineSteeringModule?.SetActive(false);
        _playerInputModule?.SetActive(false);

        // 完全停止
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // 爆発エフェクト
        if (ExplosionPrefab != null)
        {
            GameObject fx = GameObject.Instantiate(
                ExplosionPrefab,
                _vehicleController.transform.position,
                Quaternion.identity
            );

            fx.transform.localScale = Vector3.one * ExplosionScale;
        }

        // モデル非表示
        _machineSteeringModule?.SwitchShowModel(false);

        // ★ Kill 直後に即ワープ（Trigger の詰まり防止）
        RespawnImmediate();

        // 復帰処理だけコルーチンで後に回す
        _vehicleController.StartCoroutine(RespawnRoutine());
    }

    /// <summary>
    /// 即座にリスポーン地点へテレポート
    /// </summary>
    private void RespawnImmediate()
    {
        Transform point = RespawnPointManager.Instance.FindClosest(_vehicleController.transform.position);

        if (point != null)
        {
            _vehicleController.transform.SetPositionAndRotation(point.position, point.rotation);
        }

        // ワープ後も速度はゼロを維持
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// 遅れて操作を復帰させる
    /// </summary>
    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(RespawnDelay);

        // モデル復帰
        _machineSteeringModule?.SwitchShowModel(true);

        // 操作復帰
        _machineEngineModule?.SetActive(true);
        _machineSteeringModule?.SetActive(true);
        _playerInputModule?.SetActive(true);

        _isDead = false;

        var net = _vehicleController.GetComponent<NetworkMachineDestruction>();
        if (net != null && net.HasStateAuthority)
        {
            net.IsDead = false;
        }

    }
}
