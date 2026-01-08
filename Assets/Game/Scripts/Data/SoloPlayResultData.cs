using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SoloPlayResultData : MonoBehaviour
{
    public static SoloPlayResultData Instance { get; private set; }

    /// <summary> 今回のクリアタイム（秒） </summary>
    public float CurrentTime { get; private set; }

    /// <summary> トップ3タイム（秒・昇順） </summary>
    public float[] TopTimes { get; private set; } = new float[3];

    private const string TopTimeKey = "SoloPlay_TopTime_";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadTopTimes();
    }

    /// <summary>
    /// ゴール時に呼ぶタイム保存関数
    /// </summary>
    public void SetCurrentTime(float timeInSeconds)
    {
        CurrentTime = timeInSeconds;
        UpdateTopTimes(timeInSeconds);
    }

    /// <summary>
    /// トップ3を更新する（速い順）
    /// </summary>
    private void UpdateTopTimes(float newTime)
    {
        List<float> times = TopTimes
            .Where(t => t > 0f)
            .ToList();

        times.Add(newTime);

        times = times
            .OrderBy(t => t)
            .Take(3)
            .ToList();

        for (int i = 0; i < 3; i++)
        {
            TopTimes[i] = i < times.Count ? times[i] : 0f;
            PlayerPrefs.SetFloat(TopTimeKey + i, TopTimes[i]);
        }

        PlayerPrefs.Save();
    }

    private void LoadTopTimes()
    {
        for (int i = 0; i < 3; i++)
        {
            TopTimes[i] = PlayerPrefs.GetFloat(TopTimeKey + i, 0f);
        }
    }

    /// <summary>
    /// 今回が1位かどうか
    /// </summary>
    public bool IsNewRecord()
    {
        return TopTimes.Length > 0 && Mathf.Approximately(CurrentTime, TopTimes[0]);
    }

    public void ResetCurrentTime()
    {
        CurrentTime = 0.0f;
    }
}
