using UnityEngine;
using TMPro;

public class SoloPlayResultView : MonoBehaviour
{
    [Header("今回のタイム")]
    [SerializeField] private TextMeshProUGUI currentTimeText;

    [Header("トップ3")]
    [SerializeField] private TextMeshProUGUI[] topTimeTexts;

    [Header("新記録表示")]
    [SerializeField] private GameObject newRecordLabel;

    private void Start()
    {
        var data = SoloPlayResultData.Instance;

        // 今回のタイム
        currentTimeText.text = FormatTime(data.CurrentTime);

        // トップ3
        for (int i = 0; i < topTimeTexts.Length; i++)
        {
            if (i < data.TopTimes.Length && data.TopTimes[i] > 0f)
            {
                topTimeTexts[i].text = FormatTime(data.TopTimes[i]);
            }
            else
            {
                topTimeTexts[i].text = "--:--.--";
            }
        }

        // New Record
        if (newRecordLabel != null)
        {
            newRecordLabel.SetActive(data.IsNewRecord());
        }
    }

    /// <summary>
    /// 渡ってきたタイムを変換する
    /// 秒(float) → mm:ss.xx
    /// 例：254.03 → 04:14.03
    /// </summary>
    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        float seconds = time % 60f;

        return $"{minutes:00}:{seconds:00.00}";
    }
}
