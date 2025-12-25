using Fusion;
using NetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetRankingManager : NetworkBehaviour
{
    /// <summary>
    /// ランキング計算用のデータ構造
    /// </summary>
    private struct PlayerRankingData
    {
        public int Id;
        public int Lap;
        public int CoursePoint;
    }

    // 順位更新間隔(フレーム)
    const int UPDATE_INTERVAL_FRAME = 5;

    GameLauncher m_networkLauncher;
    int m_id;
    int m_intervalCounter;

    [Networked]
    int n_hostIntervalCounter { get; set; }

    [Networked, Capacity(4)]
    private NetworkDictionary<int, int> n_idToLap => default;

    [Networked, Capacity(4)]
    private NetworkDictionary<int, int> n_idToCoursePoint => default;

    [SerializeField]
    RaceProgressManager m_raceProgressManager;

    // 現在のランキング情報を受け取る (順位, 周回数, コースポイント)
    private event Action<int, int, int> m_onCurrentRanking;
    public event Action<int, int, int> OnCurrentRanking
    {
        add { m_onCurrentRanking += value; }
        remove { m_onCurrentRanking -= value; }
    }

    private void Awake()
    {

        // RaceProgressManagerの存在チェック
        if (m_raceProgressManager == null)
        {
            Debug.LogError("[NetRankingManager] RaceProgressManagerがありません");
            enabled = false;
            return;
        }
    }

    public override void Spawned()
    {
        m_networkLauncher = GameLauncher.Instance;

        m_id = m_networkLauncher.UserData.m_userID;

        if (Runner != null)
        {
            RPC_Entry(m_id);
        }
    }

    /// <summary>
    /// プレイヤーをランキングシステムに登録
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_Entry(int id)
    {
        // 重複登録防止
        if (n_idToLap.ContainsKey(id))
        {
            Debug.LogWarning($"[NetRankingManager] ID:{id} は既に登録されています");
            return;
        }

        n_idToLap.Add(id, 0);
        n_idToCoursePoint.Add(id, 0);
    }

    /// <summary>
    /// 自身のレース進行状況をホストに送信
    /// </summary>
    private void FixedUpdate()
    {
        if (Runner == null || m_raceProgressManager == null) return;

        // 送信フレームまで待つ
        m_intervalCounter++;
        if (m_intervalCounter < UPDATE_INTERVAL_FRAME) return;
        m_intervalCounter = 0;

        // ホストにデータを送る
        int currentLap = m_raceProgressManager.GetCurrentLap();
        int currentCoursePoint = m_raceProgressManager.GetCurrentCoursePoint();

        RPC_SendRaceProgress(m_id, currentLap, currentCoursePoint);
    }

    /// <summary>
    /// 全体順位更新用(ホストのみ実行)
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        n_hostIntervalCounter += 1;
        if (n_hostIntervalCounter < UPDATE_INTERVAL_FRAME) return;
        n_hostIntervalCounter = 0;

        // ランキング計算
        var ranking = RankCalculation();

        // 各プレイヤーに順位を通知
        foreach (var r in ranking)
        {
            int id = r.Key;
            int rank = r.Value;

            // データの存在チェック
            if (!n_idToLap.ContainsKey(id) || !n_idToCoursePoint.ContainsKey(id))
            {
                Debug.LogWarning($"[NetRankingManager] ID:{id} のデータが見つかりません");
                continue;
            }

            int lap = n_idToLap[id];
            int coursePoint = n_idToCoursePoint[id];

            RPC_ReceiptRanking(id, rank, lap, coursePoint);
        }
    }

    /// <summary>
    /// 順位を計算してIDとランクの辞書を返す
    /// ルール: 周回数が多い > コースポイントが大きい
    /// </summary>
    private Dictionary<int, int> RankCalculation()
    {
        Dictionary<int, int> rankings = new Dictionary<int, int>();

        // データが空の場合は空の辞書を返す
        if (n_idToLap.Count == 0)
        {
            return rankings;
        }

        // プレイヤー情報を集約
        List<PlayerRankingData> playerDataList = new List<PlayerRankingData>();

        foreach (var idToLap in n_idToLap)
        {
            int id = idToLap.Key;
            int lap = idToLap.Value;

            // コースポイントの取得
            if (!n_idToCoursePoint.TryGet(id, out int coursePoint))
            {
                Debug.LogWarning($"[NetRankingManager] ID:{id} のコースポイントが見つかりません");
                coursePoint = 0;
            }

            playerDataList.Add(new PlayerRankingData
            {
                Id = id,
                Lap = lap,
                CoursePoint = coursePoint
            });
        }

        // ソート: 周回数の降順 → コースポイントの降順
        var sortedPlayers = playerDataList
            .OrderByDescending(p => p.Lap)
            .ThenByDescending(p => p.CoursePoint)
            .ToList();

        // 順位を割り当て
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            int rank = i + 1; // 1位から開始
            rankings[sortedPlayers[i].Id] = rank;
        }

        return rankings;
    }

    /// <summary>
    /// ホストに自分の現在位置を送信
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SendRaceProgress(int id, int lap, int coursePoint)
    {
        // データの存在確認
        if (!n_idToLap.ContainsKey(id))
        {
            Debug.LogWarning($"[NetRankingManager] 未登録のID:{id} からデータ受信");
            return;
        }

        n_idToLap.Set(id, lap);
        n_idToCoursePoint.Set(id, coursePoint);
    }

    /// <summary>
    /// ホストから順位情報を受け取り、自分のデータなら通知
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ReceiptRanking(int id, int ranking, int lap, int coursePoint)
    {
        // 自分のIDでない場合は処理しない
        if (id != m_id) return;

        // イベント通知
        m_onCurrentRanking?.Invoke(ranking, lap, coursePoint);

        Debug.Log($"順位{ranking}位");
    }


}