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
    //自身のインデックス番号
    int m_index = -1;

    //生成する車
    [SerializeField]
    private NetworkPrefabRef m_vehiclePrefab;

    [SerializeField]VehicleSender m_vehicleSender;
    

    private void Start()
    {
        m_gameLauncher = GameLauncher.Instance;
    }

    public override void Spawned()
    {
        Debug.Log($"ユーザー名:{Runner.LocalPlayer}が入りました");
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
            m_index = index;
            CreateVehicle();
        }
    }

    /// <summary>
    /// 自身が使う車を作成
    /// </summary>
    private void CreateVehicle()
    {
        if(m_spowenPosition.Count <= m_index)
        {
            Debug.LogWarning($"[SpawneManager]スポーン位置が足りていません");
            return;
        }

        Vector3 position = m_spowenPosition[m_index].transform.position;

        var vehicle = Runner.Spawn(m_vehiclePrefab, position);

        m_vehicleSender.Send(vehicle.gameObject);
    }
}
