using UnityEngine;
using Fusion;
using System;
using static Unity.Collections.Unicode;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Aoi
{
    public class RoomManager : NetworkBehaviour
    {
        //ゲームランチャー
        NetWork.GameLauncher m_gameLauncher;
        [SerializeField]MainVehicleSetting m_vehicleSetting;
        [SerializeField]VehicleDataManager m_vehicleDataManager;
        public event Action OnVehicleChange;

        //自身が準備完了か
        bool m_isReady = false;
        //全員が準備完了か
        [Networked, Capacity(4)]
        private NetworkDictionary<PlayerRef, bool> n_allisReady => default;

        private void Start()
        {
            m_gameLauncher = NetWork.GameLauncher.Instance;
            if(m_vehicleDataManager == null)
            {
                Debug.LogWarning("[RoomManager]VechileDataManagerがありません");
            }


            m_gameLauncher.AddOnNetworkObjectSpawned(JoinRoom);
        }


        private void OnDestroy()
        {
            m_gameLauncher.RemoveOnNetworkObjectSpawned(JoinRoom);
        }

        /// <summary>
        /// ルームに参加
        /// </summary>
        /// <param name="runner"></param>
        private void JoinRoom(NetworkRunner runner)
        {
            //ユーザーデータから車IDを取得
            var selectID = m_gameLauncher.UserData.m_vehicleID;
            //IDから対応した配列番号取得
            int vechileIndex = m_vehicleDataManager.GetIndexToID(selectID);
            //対応した車に変更
            m_vehicleSetting.VehicleChange(vechileIndex);

            RPC_Entry(Runner.LocalPlayer);
        }


        /// <summary>
        /// 入室
        /// </summary>
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_Entry(PlayerRef user)
        {
            var userdatas = m_gameLauncher.GetAllUserData();
            //エントリー通知のユーザーがいるか確認
            if (userdatas.ContainsKey(user))
            {
                //準備完了のメンバーに追加
                n_allisReady.Add(user, false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                m_vehicleSetting.MoveDown();
                UserDataUpdate();
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                m_vehicleSetting.MoveUp();
                UserDataUpdate();
            }
        }



        /// <summary>
        /// ユーザーデータ更新
        /// </summary>
        private void UserDataUpdate()
        {
            int vehicleid = m_vehicleSetting.VehicleID;
            var userdata = m_gameLauncher.UserData;
            userdata.m_vehicleID = vehicleid;
            m_gameLauncher.UserData = userdata;
        }

        /// <summary>
        /// 準備完了ローカル
        /// </summary>
        public void Ready()
        {
            //完了フラグを反転
            m_isReady = !m_isReady;
            //ホストへの送信
            RPC_Ready(Runner.LocalPlayer, m_isReady);
        }

        /// <summary>
        /// 準備完了をネットワークに通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ready"></param>
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Ready(PlayerRef user, bool ready)
        {
            Debug.Log("準備完了受付");
            if (n_allisReady.ContainsKey(user))
            {
                n_allisReady.Set(user, ready);
                Debug.Log($"{user}の準備完了状態:{ready}");
            }

            //全員が準備完了ならゲーム開始
            if (n_allisReady.All(kvp => kvp.Value))
            {
                ChangeScene();
            }
        }

        

        /// <summary>
        /// シーン変更
        /// </summary>
        private void ChangeScene()
        {
            if (Object.HasStateAuthority)
            {
                Runner.LoadScene(SceneRef.FromIndex(Config.PLAY_SCENE), LoadSceneMode.Single);
            }
        }
    } 
}
