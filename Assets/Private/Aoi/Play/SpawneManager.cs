using UnityEngine;
using Fusion;
using System.Collections.Generic;
using NetWork;

/// <summary>
/// それぞれのプレイヤーを生成
/// </summary>
public class SpawneManager : NetworkBehaviour
{
    //ランチャー
    GameLauncher m_gameLauncher;



    //車生成場所(配列0から優先,AIも使用)
    [SerializeField]
    List<GameObject> m_spowenPosition;
    //ユーザーの生成場所管理
    [Networked, Capacity(4)]
    private NetworkArray<NetworkBool> m_spownDataUsage => default;
    //自身の配置インデックス番号
    int m_positionIndex = -1;



    //自身の車インデックス番号
    int m_vehicleIndex = 0;
    //車のデータ
    [SerializeField]
    VehicleDataManager m_vehicleDataManager;
    //生成する車
    [SerializeField]
    private NetworkPrefabRef m_vehiclePrefab;




    //車のデータを各所に送る
    [SerializeField]VehicleSender m_vehicleSender;
    

    private void Start()
    {
        m_gameLauncher = GameLauncher.Instance;
    }

    public override void Spawned()
    {
        Debug.Log($"ユーザー名:{Runner.LocalPlayer}が入りました");

        //ユーザーデータから車IDを取得
        var selectID = m_gameLauncher.UserData.m_vehicleID;
        //IDから対応した配列番号取得
        int vechileIndex = m_vehicleDataManager.GetIndexToID(selectID);

        SpawnVehicle(vechileIndex);
    }


    public void SpawnVehicle(int vechileIndex)
    {
        //使う車のデータインデックス取得
        m_vehicleIndex = vechileIndex;
        //車を生成
        RPC_RequestIndex(Runner.LocalPlayer);
    }


    /// <summary>
    /// 自身が使う場所のインデックス番号をリクエストする
    /// </summary>
    /// <param name="user"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestIndex(PlayerRef user)
    {
        for (int i = 0; i < m_spownDataUsage.Length; i++)
        {
            if (m_spownDataUsage[i]) continue;
            m_spownDataUsage.Set(i, true);
            RPC_RespondIndex(user, i);
            return;
        }
    }

    /// <summary>
    /// インデックス番号を受け取る
    /// </summary>
    /// <param name="target"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_RespondIndex(PlayerRef target, int index)
    {
        if (Runner.LocalPlayer == target)
        {
            m_positionIndex = index;
            CreateVehicle();
        }
    }

    /// <summary>
    /// 自身が使う車を作成
    /// </summary>
    private void CreateVehicle()
    {
        if(m_spowenPosition.Count <= m_positionIndex)
        {
            Debug.LogWarning($"[SpawneManager]スポーン位置が足りていません");
            return;
        }
        //生成位置決定
        Vector3 position = m_spowenPosition[m_positionIndex].transform.position;

        var data = m_vehicleDataManager.GetDataToIndex(m_vehicleIndex);


        //車を生成
        var vehicle = Runner.Spawn(data.Vehicl, position,Quaternion.identity,Runner.LocalPlayer);

        
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        if(vehicleController == null)
        {
            Debug.LogWarning($"[SpawneManager]{vehicle.gameObject}にVehicleControllerがついていません");
        }


        ////個別のデータ取得、設定
        //foreach(var factory in data.ModuleFactoryBases)
        //{
        //    vehicleController.AddSetting(factory);
        //}

        //車の初期化
        vehicleController.Initialize();

        //車のデータを送信
        m_vehicleSender.Send(vehicle.gameObject);

        //ネットワーク関連
        var networkVehicle = vehicle.GetComponent<VehicleNetwork>();
        if (networkVehicle == null) return;
        networkVehicle.RPC_SettingData(m_vehicleIndex);
        networkVehicle.RPC_Initialize();
    }
}
