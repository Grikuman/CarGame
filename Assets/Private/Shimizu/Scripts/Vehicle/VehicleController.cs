using System;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{

    [SerializeField]
    private List<VehicleModuleFactoryBase> modules = new List<VehicleModuleFactoryBase>();
    
    private Rigidbody _rb;
    private List<IVehicleModule> vehicleModuleBases = new List<IVehicleModule>();

    public float Steering { get; set; }
    public float Accelerator { get; set; }
    public float brake { get; set; }

    private void Awake()
    {
        var usedTypes = new HashSet<System.Type>();

        foreach (var moduleSetting in modules)
        {
            // モジュールを作成する
            var module = moduleSetting.Create(this);
            if (module == null) continue;

            // タイプを取得する
            Type moduleType = module.GetType();

            // 同じモジュールがないか確認する
            if (usedTypes.Contains(moduleType))
            {
                Debug.LogWarning($"Duplicate module type detected: {moduleType.Name}. Skipping.");
                continue;
            }

            // タイプを追加
            usedTypes.Add(moduleType);
            // モジュールの初期化処理
            module.Initialize(this);
            // モジュールの追加
            vehicleModuleBases.Add(module);
        }
    }

    /// <summary> 開始処理 </summary>
    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    /// <summary> 更新処理 </summary>
    private void Update()
    {
        // 各モジュールの更新処理
        foreach (var module in vehicleModuleBases)
        {
            if (module == null || !module.GetIsActive()) continue;

            module.UpdateModule();      // 更新処理
            module.FixedUpdateModule(); // 物理計算処理
        }

        this.DrawRay();
    }

    /// <summary> 指定した型のモジュールを取得 </summary>
    /// <typeparam name="T"> 取得したいモジュールの型 </typeparam>
    /// <returns> 指定した型のモジュール 存在しない場合は null </returns>
    public T Find<T>() where T : class, IVehicleModule
    {
        foreach (var module in vehicleModuleBases)
        {
            if (module is T tModule) return tModule;
        }

        Debug.LogWarning($"[VehicleController] Module of type {typeof(T).Name} not found.");
        return null;
    }

    /// <summary> 指定した型のファクトリーに対応するモジュールの設定をリセットします </summary>
    /// <typeparam name="T"> リセット対象のファクトリー型 </typeparam>
    public void ResetSettings<T>() where T : class, IVehicleModuleFactory
    {
        int n = 0;

        foreach (var module in modules)
        {
            if (module is T tModule)
            {
                module.ResetSettings(vehicleModuleBases[n]);
                return;
            }
            n++;
        }
    }

    /// <summary> ベクトルの表示 </summary>
    private void DrawRay()
    {
        Vector3 velocity = _rb.linearVelocity;

        // 各方向のベクトル
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 steeringDirection = Quaternion.Euler(0, 10, 0) * forward;

        // 各方向への速度成分（Dot積）
        float forwardMag  = Vector3.Dot(velocity, forward);
        float rightMag    = Vector3.Dot(velocity, right);
        float steeringMag = Vector3.Dot(velocity, steeringDirection);

        // スケーリング（視覚上の調整）
        float scale = 0.5f;

        // Debug表示（長さは各方向の成分に比例）
        Debug.DrawRay(transform.position, forward * forwardMag * scale, Color.green);              // 前方向
        Debug.DrawRay(transform.position, right * rightMag * scale, Color.red);                    // 横方向
        Debug.DrawRay(transform.position, steeringDirection * steeringMag * scale, Color.magenta); // ステア方向
        Debug.DrawRay(transform.position, velocity * scale, Color.blue);                           // 実際の速度ベクトル
    }


}
