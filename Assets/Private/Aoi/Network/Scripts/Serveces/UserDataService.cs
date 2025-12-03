using Fusion;
using NetWork;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UserDataService : MonoBehaviour
{
    [SerializeField] private NetworkPrefabRef m_userdataManagerPrefab;
    [SerializeField] private bool m_isLog = false;

    //自身のユーザーデータ
    private NetworkUserData m_userData = new NetworkUserData();

    [SerializeField, Tooltip("車ID")] private int m_vechileID;
    
    public NetworkUserData UserData
    {
        get { return m_userData; }
        //データの変更の際は全ユーザーに伝えるようにする
        set
        {
            m_userData = value;
            if (m_userDataManager == null)
            {
                AcquisitionReadyManager();
            }
            if (m_userDataManager != null) m_userDataManager.RPC_ChangeUserData(m_runner.LocalPlayer, UserData);
        }
    }

    private NetworkRunner m_runner;
    [SerializeField] private UserDataManager m_userDataManager;

    //データ変更時のアクション
    public event Action<IReadOnlyDictionary<PlayerRef, NetworkUserData>> OnDataChangeAction;

    /// <summary>
    /// サービスを初期化
    /// </summary>
    public void Initialize(NetworkRunner runner)
    {
        m_runner = runner;
        SpawnUserDataManager();
    }

    public void DataReset()
    {
        m_runner = null;
        m_userDataManager = null;
        m_userData.Reset();
    }

    public void PlayerLeft(PlayerRef user)
    {
        if (m_userDataManager == null) return;
        //Debug.Log($"{user}の退出を確認");
        m_userDataManager.RPC_PlayerLeft(user);
    }

    /// <summary>
    /// ユーザーデータマネージャー生成
    /// </summary>
    void SpawnUserDataManager()
    {
        if (m_runner == null)
        {
            Debug.LogWarning("[UserDataService] NetworkRunnerが初期化されていません。");
            return;
        }

        if (m_userdataManagerPrefab == null)
        {
            Debug.LogWarning("[UserDataService] Prefabがありません");
            return;
        }

        if (m_userDataManager != null) return;

        // ホスト以外はパス
        if (m_runner.IsSharedModeMasterClient)
        {
            NetworkObject spawnedObject = m_runner.Spawn(m_userdataManagerPrefab, onBeforeSpawned: (runner, obj) =>
            {
                // Flags を設定：AllowStateAuthorityOverride = ON, DestroyWhenStateAuthorityLeaves = OFF
                obj.Flags = NetworkObjectFlags.AllowStateAuthorityOverride;
                Debug.Log($"[UserDataService] UserDataManager Flags設定: {obj.Flags}");
            });

            m_userDataManager = spawnedObject.GetComponent<UserDataManager>();
            m_userDataManager.SetDataChangeAction(data => OnDataChangeAction?.Invoke(data));
            m_userDataManager.SetIDGet(id =>
            {
                m_userData.m_userID = id;
            });
            m_userDataManager.m_isLog = m_isLog;
            //自身のデータを追加
            m_userDataManager.RPC_ChangeUserData(m_runner.LocalPlayer, m_userData);

            if (m_isLog) Debug.Log("[UserDataService] UserDataManagerをSpawnしました。");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="runner"></param>
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (m_userDataManager == null) AcquisitionReadyManager();
        if (m_isLog) Debug.Log("[UserDataService] 準備完了を通知しました。");
    }

    /// <summary>
    /// ホスト以外の取得
    /// </summary>
    private void AcquisitionReadyManager()
    {
        if (m_userDataManager != null) return;

        m_userDataManager = FindFirstObjectByType<UserDataManager>();

        if (m_userDataManager == null) return;

        // NetworkBehaviourのObject nullチェック（完全に初期化されているか確認）
        if (m_userDataManager.Object == null)
        {
            m_userDataManager = null;
            return;
        }

        //自身のデータを追加
        m_userDataManager.SetDataChangeAction(data => OnDataChangeAction?.Invoke(data));
        m_userDataManager.SetIDGet(id =>
        {
            m_userData.m_vehicleID = id;
        });
        m_userDataManager.RPC_ChangeUserData(m_runner.LocalPlayer, m_userData);
        m_userDataManager.m_isLog = m_isLog;
    }

    /// <summary>
    /// 動的にデータ読み込み
    /// </summary>
    /// <returns></returns>
    public IReadOnlyDictionary<PlayerRef, NetworkUserData> GetAllUserData()
    {
        if (m_userDataManager == null) return null;
        return m_userDataManager.GetAllUserData();
    }

    /// <summary>
    /// デバッグ用: Inspectorでデータを表示
    /// </summary>
    private void Update()
    {
        // Managerのnull状態チェック
        if (m_runner != null && m_userDataManager == null)
        {
            AcquisitionReadyManager();
        }


#if UNITY_EDITOR
        // Inspector表示用にデータを同期
        m_vechileID = m_userData.m_vehicleID;
#endif
    }
}
