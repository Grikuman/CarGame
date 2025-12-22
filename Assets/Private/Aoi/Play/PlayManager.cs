using Fusion;
using NetWork;
using System.Linq;
using UnityEngine;

namespace Aoi
{
    public class PlayManager : MonoBehaviour
    {
        GameLauncher m_gameLauncher;
        [SerializeField] NetworkPrefabRef m_car;
        //車のデータ
        [SerializeField] VehicleDataManager m_vehicleDataManager;
        

        private void Start()
        {
            m_gameLauncher = GameLauncher.Instance;
            m_gameLauncher.OnPlayerJoined += SpownPlayer;
        }

        private void SpownPlayer(NetworkRunner runner,PlayerRef user)
        {
            //ユーザーデータから車IDを取得
            var selectID = m_gameLauncher.UserData.m_vehicleID;
            //IDから対応した配列番号取得
            int vechileIndex = m_vehicleDataManager.GetIndexToID(selectID);

            //IDを使って車の初期データを設定する

            if (runner.LocalPlayer == user)
            {
                runner.Spawn(m_car, new Vector3(runner.ActivePlayers.Count() * 10, 10, 0));
            }
        }
    } 
}
