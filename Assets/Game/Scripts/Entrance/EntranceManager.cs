using Fusion;
using NetWork;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aoi
{
    public class EntranceManager : MonoBehaviour
    {
        NetWork.GameLauncher m_gameLauncher;
        [SerializeField]MainVehicleSetting m_vehicleSetting;
        bool m_isConnecting = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_gameLauncher = GameLauncher.Instance;
            m_gameLauncher.OnPlayerJoined += RoomSceneMove;
            if (m_vehicleSetting == null) Debug.LogWarning("[EntranceManager]VehicleSettingがありません", gameObject);
            m_vehicleSetting.VehicleChnage();
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.UpArrow))
            {
                m_vehicleSetting.MoveDown();
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                m_vehicleSetting.MoveUp();
            }
        }

        /// <summary>
        /// ルーム接続
        /// </summary>
        public async void Conection()
        {
            if (m_isConnecting) return;
            m_isConnecting = true;

            //私用している車を取得
            int selectid = m_vehicleSetting.VehicleID;

            //ユーザーデータ更新
            var userdata = m_gameLauncher.UserData;
            userdata.m_vehicleID = selectid;
            m_gameLauncher.UserData = userdata;

            m_isConnecting = await m_gameLauncher.JoinRoom("Room1", 4);
        }

        /// <summary>
        /// ルームへ移動
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        private void RoomSceneMove(NetworkRunner runner,PlayerRef player)
        {
            if (runner.LocalPlayer != player) return;
            //待機ルームへ移動
            if (runner.IsSceneAuthority)
            {
                runner.LoadScene(SceneRef.FromIndex(Config.ROOM_SCENE), LoadSceneMode.Single);
            }
        }


        private void OnDestroy()
        {
            m_gameLauncher.OnPlayerJoined -= RoomSceneMove;
        }
    } 
}
