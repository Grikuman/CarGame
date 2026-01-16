using Fusion;
using NetWork;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Aoi
{


    public class LocalRoomManager : MonoBehaviour
    {
        [SerializeField] MainVehicleSetting m_vehicleSetting;
        [SerializeField] SelectView m_selectView;
        bool m_isConnecting = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (m_vehicleSetting == null) Debug.LogWarning("[LocalRoomManager]VehicleSettingÇ™Ç†ÇËÇ‹ÇπÇÒ", gameObject);
            m_vehicleSetting.VehicleChange(0);
        }

        private void Update()
        {
            if (Keyboard.current.upArrowKey.wasReleasedThisFrame)
            {
                int id = m_vehicleSetting.MoveDown();
                if(m_selectView)m_selectView.DataChange(id);
            }

            if (Keyboard.current.downArrowKey.wasReleasedThisFrame)
            {
                int id = m_vehicleSetting.MoveUp();
                if (m_selectView) m_selectView.DataChange(id);
            }
        }


        /// <summary>
        /// ÉãÅ[ÉÄÇ÷à⁄ìÆ
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        private void RoomSceneMove(NetworkRunner runner, PlayerRef player)
        {
            if (runner.LocalPlayer != player) return;
            ////ë“ã@ÉãÅ[ÉÄÇ÷à⁄ìÆ
            //if (runner.IsSceneAuthority)
            //{
            //    runner.LoadScene(SceneRef.FromIndex(AConfig.ROOM_SCENE), LoadSceneMode.Single);
            //}
        }

    }
}
