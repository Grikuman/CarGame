using Aoi;
using Fusion;
using NetWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///他のユーザーの車の管理
/// </summary>
public class OtherVehicleManager : NetworkBehaviour
{
    //ゲームランチャー
    NetWork.GameLauncher m_gameLauncher;
    //ユーザーと使っているインデックス番号
    Dictionary<PlayerRef, int> m_usertoIndex = new Dictionary<PlayerRef, int>();
    private GameObject[] m_otherVehicle = new GameObject[3];
    //他のユーザーの車を設置する場所
    [SerializeField]private GameObject[] m_carPositions = new GameObject[3];
    //場所の使用状況
    private bool[] m_indexUsage = new bool[3];

    [SerializeField] VehicleDataManager m_vehicleSetting;

    
    void Start()
    {
        m_gameLauncher = NetWork.GameLauncher.Instance;
        m_gameLauncher.AddOnNetworkObjectSpawned(Entry);
        m_gameLauncher.AddOnUserDataChange(UpdateUserData);
        m_gameLauncher.OnPlayerLeft += RemoveUser;
    }

   

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (m_gameLauncher != null)
        {
            m_gameLauncher.RemoveOnNetworkObjectSpawned(Entry);
            m_gameLauncher.RemoveOnDataChangeAction(UpdateUserData);
            m_gameLauncher.OnPlayerLeft -= RemoveUser;
        }
    }

    public void Entry(NetworkRunner runner)
    {
        Debug.Log("ルームに入室しました");
        RPC_EntryNotifi(runner.LocalPlayer);
        LoadOtherUserData();
    }

    /// <summary>
    ///他のユーザーが入ったのを通知
    /// </summary>
    /// <param name="user"></param>
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_EntryNotifi(PlayerRef user)
    {
        StartCoroutine(AddUserData(user));
    }

    /// <summary>
    /// ユーザーデータからユーザーを追加
    /// </summary>
    private void LoadOtherUserData()
    {
        var userdatas = m_gameLauncher.GetAllUserData();
        foreach (var userdata in userdatas)
        {
            StartCoroutine(AddUserData(userdata.Key));
        }
    }


    /// <summary>
    /// ユーザーを追加 データ送信ラグのため待つ
    /// </summary>
    /// <param name="user"></param>
    IEnumerator AddUserData(PlayerRef user)
    {
        yield return new WaitForSeconds(0.3f);

        if (m_usertoIndex.ContainsKey(user)) yield break;
        //自身ならパス
        if (user == Runner.LocalPlayer) yield break;

        //入室したユーザーデータを取得
        var userdatas = m_gameLauncher.GetAllUserData();
        if (userdatas.ContainsKey(user))
        {
            var userdata = userdatas[user];

            //使える番号を取得
            int index = GetIndexNum();
            //車更新
            UpdateVehicle(index, userdata.m_vehicleID);
            //ユーザーとインデックス番号を紐づけ
            m_usertoIndex[user] = index;
        }
    }

    /// <summary>
    /// 空いているインデックス番号を返す
    /// </summary>
    /// <returns></returns>
    private int GetIndexNum()
    {
        for (int i = 0; i < m_indexUsage.Length; i++)
        {
            if (m_indexUsage[i]) continue;
            m_indexUsage[i] = true;

            return i;
        }
        //全て使っている場合は-1
        return -1;
    }

    /// <summary>
    /// 他のユーザーの更新に合わせて更新
    /// </summary>
    /// <param name="userdatas"></param>
    private void UpdateUserData(IReadOnlyDictionary<PlayerRef, NetworkUserData> userdatas)
    {
        foreach (var userdata in userdatas)
        {
            var user = userdata.Key;

            //自身ならパス
            if (user == Runner.LocalPlayer) continue;
            //キーとして内ならパス
            if (!m_usertoIndex.ContainsKey(user)) continue;

            //入室したユーザーデータを取得

            //使える番号を取得
            int index = m_usertoIndex[user];
            //車更新
            UpdateVehicle(index, userdata.Value.m_vehicleID);
        }
    }

    /// <summary>
    /// 対応の車を更新
    /// </summary>
    /// <param name="index"></param>
    /// <param name="vehicleID"></param>
    private void UpdateVehicle(int index,int vehicleID)
    {
        //先においてあるものがあれば破壊
        if (m_otherVehicle[index] != null) Destroy(m_otherVehicle[index]);

        //対応した車取得
        var vehicleData = m_vehicleSetting.GetDataToID(vehicleID);
        //更新
        m_otherVehicle[index] = Instantiate(vehicleData.Model,
            m_carPositions[index].transform.position,
            m_carPositions[index].transform.rotation);
    }

    /// <summary>
    /// ユーザーが抜けた際
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="user"></param>
    private void RemoveUser(NetworkRunner runner, PlayerRef user)
    {
        if(m_usertoIndex.ContainsKey(user))
        {
            int index = m_usertoIndex[user];

            m_usertoIndex.Remove(user);

            m_indexUsage[index] = false;

            if (m_otherVehicle[index]) Destroy(m_otherVehicle[index]);
        }
    }
}
