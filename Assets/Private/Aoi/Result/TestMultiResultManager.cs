using NetWork;
using TMPro;
using UnityEngine;

public class TestMultiResultManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        var launcher = GameLauncher.Instance;
        if (launcher == null)
        {
            enabled = false;
            return;
        }
        var userdata = launcher.UserData;

        m_text.text = $"Rank{userdata.m_ranking} Time{userdata.m_raceTime}";

        await launcher.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
