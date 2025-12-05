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

    public void UpdateModule()
    { 

    }
    public void FixedUpdateModule() 
    {

    }

    public void ResetModule(MachineRespawnModuleData data)
    {
        RespawnDelay = data.RespawnDelay;
        ExplosionPrefab = data.ExplosionPrefab;
        ExplosionScale = data.ExplosionScale;
        _isDead = false;
    }

    /// <summary>
    /// MachineDestructionModuleから呼び出される関数 
    /// </summary>
    public void Kill()
    {
        if (_isDead) return;
        _isDead = true;

        // 操作関連モジュールを一時的に無効化する
        _machineEngineModule?.SetActive(false);
        _machineSteeringModule?.SetActive(false);
        _playerInputModule?.SetActive(false);

        // 速度を止めておく
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // 爆発エフェクトの生成
        if (ExplosionPrefab != null)
        {
            GameObject fx = GameObject.Instantiate(
                ExplosionPrefab,
                _vehicleController.transform.position,
                Quaternion.identity
            );

            // スケール変更
            fx.transform.localScale = Vector3.one * ExplosionScale;
        }

        // 一時的にマシンを非表示にする
        _machineSteeringModule?.SwitchShowModel(false);

        _vehicleController.StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(RespawnDelay);
        Respawn();
    }

    private void Respawn()
    {
        // 最寄りのリスポーン地点へワープする
        Transform point = RespawnPointManager.Instance.FindClosest(_vehicleController.transform.position);

        if (point)
        {
            _vehicleController.transform.SetPositionAndRotation(point.position, point.rotation);
        }

        // マシンを再表示する
        _machineSteeringModule?.SwitchShowModel(true);

        // 完全に停止させる
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // 操作モジュールの再有効化
        _machineEngineModule?.SetActive(true);
        _machineSteeringModule?.SetActive(true);
        _playerInputModule?.SetActive(true);

        _isDead = false;
    }
}
