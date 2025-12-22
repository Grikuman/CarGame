using UnityEngine;
using Fusion;
using System;
using static Unity.Collections.Unicode;
using UnityEngine.SceneManagement;

namespace Aoi
{
    public class RoomManager : NetworkBehaviour
    {
        //ゲームランチャー
        NetWork.GameLauncher m_gameLauncher;
        [SerializeField]MainVehicleSetting m_vehicleSetting;
        [SerializeField]VehicleDataManager m_vehicleDataManager;
        public event Action OnVehicleChange;

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

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (Object.HasStateAuthority)
                {
                    Runner.LoadScene(SceneRef.FromIndex(Config.PLAY_SCENE), LoadSceneMode.Single);
                }
            }
        }

        public override void FixedUpdateNetwork()
        {
            
        }

        private void UserDataUpdate()
        {
            int vehicleid = m_vehicleSetting.VehicleID;
            var userdata = m_gameLauncher.UserData;
            userdata.m_vehicleID = vehicleid;
            m_gameLauncher.UserData = userdata;
        }
    } 
}
