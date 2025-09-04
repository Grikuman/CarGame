using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RaceSystem : MonoBehaviour
{

    public TextMeshProUGUI timeText;

    public int count;
    public bool cangoal, goalnow = false, StartGoalLine = false;
    private float seconds, minutes;

    void Update()
    {
        timer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint")//チェックポイントに触れた
        {
            Destroy(other.gameObject);
            count += 1;
        }
        if (other.gameObject.tag == "Line")//スタートラインに触れた
        {
            if (count >= 4)//チェックポイントをすべて通ったか
            {
                Debug.Log("GOAL!");
                StartGoalLine = false;
                goalnow = true;
                SceneManager.LoadScene("ResultScene");
            }
            else
            {
                StartGoalLine = true;
            }
        }
    }

    void timer()
    {
        if (StartGoalLine)
        {
            seconds += Time.deltaTime;
        }

        if (seconds >= 60)
        {
            minutes++;
            seconds -= 60;
        }

        if (!goalnow)
        {
            //ゴールしてない
            timeText.text = "Time " + minutes.ToString("00") + " : " + ((int)seconds).ToString("00");
        }
        else
        {
            //ゴールした
            timeText.text = "GoalTime  " + minutes.ToString("00") + " : " + ((int)seconds).ToString("00");
        }
    }
}