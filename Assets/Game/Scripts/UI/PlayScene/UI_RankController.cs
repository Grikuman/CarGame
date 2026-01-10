using NetWork;
using UnityEngine;
using UnityEngine.UI;

public class UI_RankController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private NetRankingManager rankingManager;


    [Header("Current Rank Images (Index = Rank-1)")]
    [SerializeField] private Image[] currentRankImages; // 1～4位

    [Header("Max Player Images (Index = PlayerCount-1)")]
    [SerializeField] private Image[] maxPlayerImages; // 1～4人

    private int _currentRank = -1;

    private void Awake()
    {
        if (rankingManager == null)
        {
            Debug.LogError("[UI_RankController] NetRankingManager が設定されていません");
            enabled = false;
            return;
        }

        rankingManager.OnCurrentRanking += OnRankingUpdated;

        // 最大人数は最初に一度表示
        UpdateMaxPlayerUI();
    }

    private void OnDestroy()
    {
        if (rankingManager != null)
        {
            rankingManager.OnCurrentRanking -= OnRankingUpdated;
        }
    }

    // ================================
    // 順位更新イベント
    // ================================
    private void OnRankingUpdated(int rank, int lap, int coursePoint)
    {
        _currentRank = rank;
        UpdateCurrentRankUI();
    }

    // ================================
    // 現在順位UI
    // ================================
    private void UpdateCurrentRankUI()
    {
        if (_currentRank <= 0) return;

        DisableAll(currentRankImages);

        int index = _currentRank - 1;
        if (index >= 0 && index < currentRankImages.Length)
        {
            currentRankImages[index].enabled = true;
        }
    }

    // ================================
    // 最大人数UI
    // ================================
    private void UpdateMaxPlayerUI()
    {
        var launcher = GameLauncher.Instance;
        if (launcher == null) return;

        DisableAll(maxPlayerImages);

        int maxPlayer = launcher.GetStartingNumber();
        int index = maxPlayer - 1;

        if (index >= 0 && index < maxPlayerImages.Length)
        {
            maxPlayerImages[index].enabled = true;
        }
    }

    // ================================
    // 共通処理
    // ================================
    private void DisableAll(Image[] images)
    {
        if (images == null) return;

        foreach (var img in images)
        {
            if (img != null) img.enabled = false;
        }
    }
}
