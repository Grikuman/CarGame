using Fusion;
using NetWork;
using System.Linq;
using UnityEngine;

namespace Aoi
{
    public class PlayManager : NetworkBehaviour
    {
        GameLauncher m_gameLauncher;
        bool m_isStarted = false;

        // 開始時間（全クライアントで同期）
        [Networked] private double m_startTime { get; set; } = 0;

        [SerializeField] RaceManager m_raceManager;

        private void Start()
        {
            m_gameLauncher = GameLauncher.Instance;

            // nullチェックで安全性を確保
            if (m_gameLauncher != null)
            {
                m_gameLauncher.AddOnAllUserReady(OnAllUserReady);
            }
            else
            {
                Debug.LogError("GameLauncher instance is null");
            }

        }

        private void OnDestroy()
        {
            if (m_gameLauncher != null)
            {
                m_gameLauncher.RemoveOnAllUserReady(OnAllUserReady);
            }
        }

        private void FixedUpdate()
        {
            if (Runner == null) return;

            // 開始時刻に達したかチェック
            if (!m_isStarted && m_startTime > 0 && Runner.SimulationTime >= m_startTime)
            {
                m_isStarted = true;
                OnGameStart();
            }
        }


        /// <summary>
        /// ゲーム開始時に全クライアントで呼ばれる処理
        /// </summary>
        private void OnGameStart()
        {
            if(m_raceManager != null)
            {
                m_raceManager.StartRaceSequence();
            }
        }

        /// <summary>
        /// 全ユーザーが揃った時の処理
        /// </summary>
        private void OnAllUserReady()
        {
            if (!HasStateAuthority) return;

            m_startTime = Runner.SimulationTime + 2.0;
        }
    }
}