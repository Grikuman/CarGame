using Fusion;
using UnityEngine;

/// <summary>
/// ネットワーク仲介用
/// </summary>
public class VehicleNetwork : NetworkBehaviour
{
    [SerializeField] VehicleController m_vehicleController;
    [SerializeField]VehicleDataManager m_vehicleDataManager;



    private void Awake()
    {
        if(m_vehicleDataManager == null)
        {
            Debug.LogWarning("[VehicleNetwork]VehicleDataManagerを設定してください", gameObject);
            return;
        }

        if (m_vehicleController == null) m_vehicleController = GetComponent<VehicleController>();
        if (m_vehicleController == null)
        {
            Debug.LogWarning("[VehicleNetwork]VehicleControllerがありません", gameObject);
            Destroy(this);
            return;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All,InvokeLocal = false)]
    public void RPC_SettingData(int dataindex)
    {
        //個別のデータ取得、設定
        var data = m_vehicleDataManager.GetDataToIndex(dataindex);
        foreach (var factory in data.ModuleFactoryBases)
        {
            m_vehicleController.AddSetting(factory);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Initialize()
    {
        Debug.Log("呼ばれた");
        m_vehicleController.Initialize();
    }
}
