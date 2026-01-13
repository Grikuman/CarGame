using TMPro;
using UnityEngine;

public class TestRankingView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_text;
    [SerializeField]
    NetRankingManager m_netranking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (m_netranking)
        {
            m_netranking.OnCurrentRanking += UpdateRanking;
        }
    }

    private void OnDestroy()
    {
        if(m_netranking)
        {
            m_netranking.OnCurrentRanking -= UpdateRanking;
        }
    }

    private void UpdateRanking(int rank,int lap, int point)
    {
        m_text.text = $"Rank:{rank}  Lap:{lap}  CourcePoint{point}";
    }
}
