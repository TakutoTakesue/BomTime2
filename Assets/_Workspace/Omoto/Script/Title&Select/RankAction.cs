using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを扱うのに必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class RankAction : MonoBehaviour
{

    public Text[] txtRank;
    float Elapsed = 0.0f;

    public Text txtNavi;
    public Text txtScore;

    float Score = Play_TimeManager.Score;

    // Start is called before the first frame update
    void Start()
    {
        for (int idx = 1; idx <= 5; idx++)
        {
            if (PlayerPrefs.GetFloat("R" + idx) >= float.MaxValue)
            {
                txtRank[idx - 1].text = "_.__s";
            }
            else
            {
                txtRank[idx - 1].text = PlayerPrefs.GetFloat("R" + idx).ToString("f2") + "s";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;

        Elapsed %= 1.0f;

        txtNavi.text = (Elapsed < 0.8f) ? "Press A Title" : "";

        txtScore.text = "TIME:" + Score.ToString("f1") + "s";

        if (Input.GetButtonDown("BtnA"))
        {
                SceneManager.LoadScene("Title");
            
        }
    }
}
