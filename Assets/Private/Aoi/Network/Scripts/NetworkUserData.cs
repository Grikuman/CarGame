using Fusion;
using UnityEngine;

namespace NetWork
{
    /// <summary>
    /// ネットワーク用のユーザーデータ
    /// </summary>
    public struct NetworkUserData : INetworkStruct
    {
        public int m_userID;
        public int m_vehicleID;//ネット用ID
        public NetworkString<_16> m_name;//名前
        public int m_ranking;
        public float m_raceTime;

        public void Reset()
        {

        }
    } 
}
